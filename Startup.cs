using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication("CookieAuth")
            //    .AddCookie("CookieAuth", config => {
            //        config.LoginPath = "/Home/Authenticate";
            //        config.Cookie.Name = "GrandMa.Cookie";
            //    });

            services.AddControllersWithViews();

            //Use The In Memory DB To Store User Data
            services.AddDbContext<ApplicationDBContext>(config =>
            {
                config.UseInMemoryDatabase("IdentityDB");
            });

            //Configure The Identity Infrastructure
            services.AddIdentity<IdentityUser, IdentityRole>()
                //Extend The Identity To user EntityFrameWork DB
                .AddEntityFrameworkStores<ApplicationDBContext>()
                    //It is Used For Reset Password or Change Email
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Home/Login";
                config.Cookie.Name = "Identity.Cookie";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            //Who You are ? 
            app.UseAuthentication();

            //are you Allowed ? 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
