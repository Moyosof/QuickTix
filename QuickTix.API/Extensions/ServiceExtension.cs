using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuickTix.Repo.Data;
using QuickTix.Service.Filters.ActionFilters;
using QuickTix.Service.Interfaces;
using QuickTix.Service.Services;
using System.Reflection;
using System.Text;

namespace QuickTix.API.Extensions
{
    public static class ServiceExtension
    {
        private readonly static string SpecificOrigin = "Specific_OriginCORs";
        private readonly static string AnyOrigin = "Any_origin";
        private readonly static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static void ConfigureRepositoryAndServices(this IServiceCollection services, IConfiguration Configuration)
        {
            #region Configure Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<ValidationFilterAttribute>();

            #endregion
        }

        public static void ConfigureModelState(this IServiceCollection services)
        {
            #region ModelState

            services.Configure<ApiBehaviorOptions>(options =>
            {

                options.SuppressModelStateInvalidFilter = true;
            });

            #endregion
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            #region IDENTITY
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            #endregion
        }

        public static void ConfigureMapping(this IServiceCollection services)
        {
            #region MAPPINGS
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            var mapperConfig = new MapperConfiguration(map =>
            {
                //map.AddProfile<TeacherMappingProfile>();
                //map.AddProfile<StudentMappingProfile>();
                //map.AddProfile<UserMappingProfile>();
            });

            services.AddSingleton(mapperConfig.CreateMapper());

            #endregion
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            #region CONTROLLERS
            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("30SecondsCaching", new CacheProfile
                {
                    Duration = 30
                });
            });
            #endregion
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            #region JWT
            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = Configuration["JwtSettings:Audience"],
                ValidIssuer = Configuration["JwtSettings:Site"],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"])),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(TokenValidationParameters);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = TokenValidationParameters;
            });
            #endregion
        }


        public static void ConfigureSwaggerGen(this IServiceCollection services, string xmlPath = null)
        {
            #region SWAGGERGEN
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Quick Tix API",
                    Version = "v1",
                    Description = "Quick Tix API Services.",
                    Contact = new OpenApiContact
                    {
                        Name = "Quick Tix."
                    },
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


                // XML DOCUMENTATION
                if (string.IsNullOrWhiteSpace(xmlPath))
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                }

                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            #endregion
        }

        public static string ConfigureCorsSpecificOrigin(this IServiceCollection services, string[] origins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: SpecificOrigin, builder => builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod());
            });

            return SpecificOrigin;
        }

        public static void ConfigureCORS(this IServiceCollection services, IConfiguration Configuration)
        {
            #region Cors

            services.AddCors(options =>
            {
                options.AddPolicy(name: AnyOrigin, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            #endregion
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            #region API VERSIONING
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; //this will cause our api to default to verion 1.0

                //Default verion 1.0
                options.DefaultApiVersion = ApiVersion.Default; //new ApiVersion(1, 0); //ApiVersion.Default;

                options.ApiVersionReader = ApiVersionReader.Combine(new MediaTypeApiVersionReader("version"),
                                                                    new HeaderApiVersionReader("x-api-version"));

                options.ReportApiVersions = true; // Will provide the different api version which is available for the client
            });
            #endregion
        }

        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration Configuration)
        {
            #region DB Connection
            try
            {
                if (env == "Production")
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Production"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Production")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Staging")
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Staging"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Staging")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Development")
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Configuration.GetConnectionString("WebApiDatabase_Development"), serverVersion: ServerVersion.AutoDetect(Configuration.GetConnectionString("WebApiDatabase_Development")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }
            }
            catch (Exception)
            {

            }

            //Reisty dbcontext
            try
            {
                if (env == "Production")
                {
                    services.AddDbContext<QuickTixDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Production"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Production")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Staging")
                {
                    services.AddDbContext<QuickTixDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Staging"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Staging")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Development")
                {
                    services.AddDbContext<QuickTixDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Configuration.GetConnectionString("WebApiDatabase_Development"), serverVersion: ServerVersion.AutoDetect(Configuration.GetConnectionString("WebApiDatabase_Development")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }
            }
            catch (Exception)
            {

            }
            #endregion
        }
    }
}
