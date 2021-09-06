using IdentityExample.AuthorizationPolicy;
using IdentityExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

            
            services.AddAuthorization(config =>
            {
                //This is how the Default Policy is Built

                //var policyBuilder = new AuthorizationPolicyBuilder();
                //var defaultPolicy = policyBuilder
                //.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.Email)
                //.Build();

                //config.DefaultPolicy = defaultPolicy;

                //Creating Our Own Policy
                config.AddPolicy("Alaa.Policy", policyBuilder =>
                {
                    //To add Policy We need Requirments and to add requirments we need handlers
                    policyBuilder.AddRequirements(new CustomeRequirmentClaims(ClaimTypes.Name));
                });


                config.AddPolicy("Admin", policyBuilder => {
                    policyBuilder.RequireClaim(ClaimTypes.Role, "Admin");
                });

            });

            services.AddScoped<IAuthorizationRequirement, CustomeRequirmentClaims>();


            services.AddControllersWithViews(config=>
            {
                var builder = new AuthorizationPolicyBuilder();
                var defaultPolicy = builder
                                     .RequireAuthenticatedUser()
                                     .Build();

                //Adding Global Authorization for the project
                config.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });

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
