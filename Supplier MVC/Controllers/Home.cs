using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Supplier_MVC.Context;
using Supplier_MVC.Models;

namespace Supplier_MVC.Controllers
{
    public class Home : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly Microsoft.AspNetCore.Identity.SignInManager<Models.SupplierUser> signInManager;

        public Home(DatabaseContext databaseContext, SignInManager<Models.SupplierUser> signInManager)
        {
            Console.WriteLine($"Initialized home controller");
            _databaseContext = databaseContext;
            this.signInManager = signInManager;
            _databaseContext.Database.EnsureCreated();

        }

        [HttpGet("/")]
        public IActionResult Root()
        {
            Console.WriteLine($"Initialized home controller");

            Response.Redirect("./login");
            return Content(string.Empty);
        }

        [Authorize]
        [HttpGet("/home")]
        public IActionResult Index()
        {
            ViewData["Products"] = _databaseContext.Products.ToList();
            return View();
        }

        [Authorize]
        [HttpPost("/add")]
        public async Task<IActionResult> Index(string name, string description, string unit, string id)
        {
            var intId = int.Parse(id);

            if (intId > 0)
            {
                // Edit item.
                var found = _databaseContext.Products.FirstOrDefault(x => x.ProductId == intId);
                if (found is { })
                {
                    found.Name = name;
                    found.Description = description;
                    found.Unit = unit;
                }
            }
            else
            {
                // Add item.
                _databaseContext.Products.Add(new ProductsModel()
                {
                    ProductId = _databaseContext.Products.Count() + 1,
                    Name = name,
                    Description = description,
                    Unit = unit
                });
            }

            await _databaseContext.SaveChangesAsync();

            return Content("ok");
        }

        [Authorize]
        [HttpPost("/remove")]
        public async Task<IActionResult> Index(string id)
        {
            var found =
                _databaseContext.Products.FirstOrDefault(x => x.ProductId == int.Parse(id));

            if (found is { })
                _databaseContext.Products.Remove(found);

            await _databaseContext.SaveChangesAsync();

            return Content("ok");
        }

        [HttpGet("/logout")]
        [HttpPost("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectPermanent("./login");
        }

    }
}