using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace PERI.Prompt.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            services.AddAuthentication("MyCookieMiddlewareInstance")
            .AddCookie("MyCookieMiddlewareInstance", options =>
            { 
                options.AccessDeniedPath = "/Main/Forbidden/";
                options.LoginPath = "/SignIn/";
            });

            // ReCaptcha requirements
            services.AddSingleton<IConfigurationRoot>(Core.Setting.Configuration);
            services.AddSingleton<IConfiguration>(Core.Setting.Configuration);
            services.AddScoped<BLL.ValidateReCaptchaAttribute>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public Startup(IHostingEnvironment env)
        {
            env.ConfigureNLog("nlog.config");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Core.Setting.Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                // Permalinks support
                routes.MapRoute(
                    name: "CmsRoute",
                    template: "{*permalink}",
                    defaults: new { controller = "Page", action = "Index" },
                    constraints: new { permalink = new BLL.CmsUrlConstraint() }
                );

                // Areas support
                routes.MapRoute(
                  name: "areaRoute",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
