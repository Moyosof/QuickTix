using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Versioning;

using QuickTix.API.Extensions;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace QuickTix.API
{
    public class Startup
    {
        private readonly static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public static string[] _origins { get; set; }
        public string SpecificOrigin { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            if (env.Equals("Production", StringComparison.OrdinalIgnoreCase))
            {
                _origins = configuration.GetSection("CorsOrigin:MobileApp").Get<string[]>();
            }

            if (env.Equals("Staging", StringComparison.OrdinalIgnoreCase))
            {
                _origins = configuration.GetSection("CorsOrigin:MobileApp").Get<string[]>();
            }

            if (env.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                _origins = configuration.GetSection("CorsOrigin:MobileApp").Get<string[]>();
            }
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureRepositoryAndServices(Configuration);
            services.ConfigureIdentity();
            services.ConfigureDatabaseConnection(Configuration);
            services.ConfigureJWT(Configuration);

            services.ConfigureResponseCaching();
            services.ConfigureModelState();
            services.ConfigureControllers();


            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.ConfigureSwaggerGen(xmlPath);

            services.ConfigureMapping();
            services.ConfigureJWT(Configuration);

            services.ConfigureApiVersioning();
            services.ConfigureCORS(Configuration);
            SpecificOrigin = services.ConfigureCorsSpecificOrigin(_origins);

            #region Authorization Filter

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();
            #endregion

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            services.AddRouting(opt =>
            {
                opt.LowercaseUrls = true;
                opt.LowercaseQueryStrings = false;
            });

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reisty.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            //app.Use(async (context, next) => {
            //    context.Request.EnableBuffering();
            //    await next();
            //});

            app.UseCors(SpecificOrigin);

            app.UseAuthorization();
            //app.ConfigureExceptionHandler();

            app.UseSwaggerAuthorized();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickTix.API v1");
                c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
