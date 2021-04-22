using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskmanWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSpaStaticFiles(options => {
                options.RootPath = "ClientApp";
            });

            // setup authentication using cookies
            AuthenticationBuilder auth = services.AddAuthentication("Default");

            auth.AddCookie("Default", options => {
                options.Cookie.Name = "TaskmanAuthID";
                options.LoginPath = "/login";
            });


            // setup authorization
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // use authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // map all the api controller endpoints
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            // use the spa in the ClientApp folder
            app.UseSpaStaticFiles();
            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";
                spa.Options.DefaultPage = "index.html";
            });
        }
    }
}
