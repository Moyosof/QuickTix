using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuickTix.API.Data;
using QuickTix.API.Entities;
using QuickTix.API.Filters.ActionFilters;
using QuickTix.API.Helpers;
using QuickTix.API.Repositories.Interfaces;
using QuickTix.API.Repositories.Services;
using System.Reflection;
using System.Text;

namespace QuickTix.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureRepositoryAndServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserAuth, UserAuth>();


            services.AddScoped<ValidationFilterAttribute>();

        }

        public static void ConfigureLoggerService(this IServiceCollection services) =>
                  services.AddScoped<ILoggerManager, LoggerManager>();


        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<QuickTixDbContext>(options =>
            {
                options.UseMySql(connectionString: Configuration.GetConnectionString("ApplicationConnectionString"), serverVersion: ServerVersion.AutoDetect(Configuration.GetConnectionString("ApplicationConnectionString")), mySqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.UseNetTopologySuite();
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });
        }

        public static void ConfigureMapping(this IServiceCollection services)
        {
            #region MAPPINGS
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            var mapperConfig = new MapperConfiguration(map =>
            {

                map.AddProfile<MappingProfiles>();
            });

            services.AddSingleton(mapperConfig.CreateMapper());

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

            }).AddEntityFrameworkStores<QuickTixDbContext>().AddDefaultTokenProviders();

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

        

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["jwtConfig:validIssuer"],
                    ValidAudience = configuration["jwtConfig:validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtConfig:secret"]))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "QuickTix.API",
                    Version = "v1",
                    Description = "A ticket reservation platform",
                    TermsOfService = new Uri("https://reistry-landing.vercel.app"),
                    Contact = new OpenApiContact
                    {
                        Name = "Quick Tix Services",
                        Email = "reistyapp@gmail.com",
                        Url = new Uri("https://reistry-landing.vercel.app"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Quick API LICX",
                        Url = new Uri("https://reistry-landing.vercel.app"),
                    }
                });

                //opt.OperationFilter<SwaggerHeaderFilter>(); // setting header input for api request

                //Enable Jwt Authorization in Swagger
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and your valid token in the text input below. \r\n\r\nExample: \"Bearer eyJhnbGciOrNwi78gGhiLLiUjo9A8dXCVBk9\""
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

    }
}
