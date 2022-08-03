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
                .ConfigureServices(services => services.AddEntityFrameworkSqlite())
                .ConfigureServices(services =>
                {

                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static void Main(string[] args) => new Program().StartAsync(args).GetAwaiter().GetResult();
    }
}