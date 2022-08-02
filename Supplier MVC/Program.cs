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
    public class Program
    {
        public async Task StartAsync(string[] args)
        {
            //await Supplier_MVC.Database.LocalDatabase.ConnectAsync();
            //Supplier_MVC.Database.LocalDatabase.InitializeTables();

            CreateHostBuilder(args).Build().Run();
        }

        public IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(servers => servers.AddEntityFrameworkSqlite()
                    .AddDbContext<Context.DatabaseContext>().Configure<IdentityOptions>(options =>
                    {
                        // Password settings.
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = true;
                        options.Password.RequiredLength = 4;
                        options.Password.RequiredUniqueChars = 1;

                        // Lockout settings.
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.AllowedForNewUsers = true;

                        // User settings.
                        options.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        options.User.RequireUniqueEmail = false;
                    }).ConfigureApplicationCookie(options =>
                    {
                        // Cookie settings
                        options.Cookie.HttpOnly = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                        options.LoginPath = "/login";
                        options.AccessDeniedPath = "/accessdenied";
                        options.SlidingExpiration = true;
                    }).AddIdentity<IdentityUser, IdentityRole>(options => { })
                    .AddEntityFrameworkStores<DatabaseContext>())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static void Main(string[] args) => new Program().StartAsync(args).GetAwaiter().GetResult();
    }
}