using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Supplier_MVC.Context;

namespace Supplier_MVC
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
            services.AddControllersWithViews();

            services.AddDbContext<DatabaseContext>();

            services.Configure<IdentityOptions>(o =>
            {
                // Password settings.
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 4;
                o.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.AllowedForNewUsers = true;

                // User settings.
                o.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 _";
                o.User.RequireUniqueEmail = false;
            });

            services.AddDefaultIdentity<Models.SupplierUser>(o =>
                o.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<DatabaseContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(5);

                options.LoginPath = "/login";
                options.AccessDeniedPath = "/login";
                options.LogoutPath = "/logout";

                options.SlidingExpiration = true;
            });


            // services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Root}/{id?}");

                endpoints.MapGet("/debug/routes", async ctx =>
                {
                    await ctx.Response.WriteAsync(string.Join(", ", endpoints.DataSources.SelectMany(x => x.Endpoints)));
                });

                endpoints.MapControllers();
            });

        }
    }
}