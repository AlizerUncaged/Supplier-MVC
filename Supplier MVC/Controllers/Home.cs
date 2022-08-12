using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Add(ProductsModel product)
        {
            // Convert image byte to byte array.
            if (product.Image is { Length: > 0 })
            {
                IFormFile file = product.Image;

                long length = file.Length;
                if (length < 0)
                    return BadRequest();

                using var fileStream = file.OpenReadStream();
                product.Thumbnail = new byte[file.Length];
                await fileStream.ReadAsync(product.Thumbnail, 0, (int)file.Length);
            }

            var existingProduct = _databaseContext.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (existingProduct is null)
                await _databaseContext.Products.AddAsync(product);
            else
            {
                product.Thumbnail = existingProduct.Thumbnail;
                _databaseContext.Entry(existingProduct).CurrentValues.SetValues(product);
            }

            //
            // if (_databaseContext.Entry(product).State == EntityState.Detached)
            // {
            //     await _databaseContext.Products.AddAsync(product);
            // }
            // else _databaseContext.Entry(product).State = EntityState.Modified;

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