using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Supplier_MVC.Models;

namespace Supplier_MVC.Context
{
    public class DatabaseContext : IdentityDbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<ProductsModel> Products { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<PurchaseOrderDetailsModel> PurchaseOrderDetails { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<PurchaseOrderHeadersModel> PurchaseOrderHeaders { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<SupplierModel> Suppliers { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<SupplierUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Database.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SupplierModel>(x =>
            {
                x.Property(e => e.SupplierId).ValueGeneratedNever();
            });

            base.OnModelCreating(builder);
        }
    }
}