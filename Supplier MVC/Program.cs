using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Supplier_MVC
{
    public class Program
    {
        public async Task StartAsync(string[] args)
        {
            await Supplier_MVC.Database.LocalDatabase.ConnectAsync();
            Supplier_MVC.Database.LocalDatabase.InitializeTables();

            CreateHostBuilder(args).Build().Run();
        }

        public IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        public static void Main(string[] args) => new Program().StartAsync(args).GetAwaiter().GetResult();
    }
}