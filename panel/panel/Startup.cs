using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using panel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace panel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.Cookie.HttpOnly = true;
                       options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                       options.LoginPath = "/login";
                       options.AccessDeniedPath = "/Home/AccessDenied";
                       options.SlidingExpiration = true;
                   });
            services.AddControllersWithViews();
            services.ConfigureServices();
            services.AddHttpClient();

            services.AddSession(opt =>
            {
                opt.IOTimeout = TimeSpan.FromMinutes(30);
                opt.IdleTimeout = TimeSpan.FromMinutes(30);
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseHttpsRedirection();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                await next();
                if (context.Response.StatusCode == 404)
                {
                    var routeNF = "notfound";
                    context.Request.Path = routeNF;
                    await next();
                }
                else if (context.Response.StatusCode == 500)
                {
                    var routeISE = "ise";
                    context.Request.Path = routeISE;
                    await next();
                }
            });
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(opt =>opt.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
