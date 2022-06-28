using Microsoft.AspNetCore.Mvc;

namespace Supplier_MVC.Controllers
{
    public class Home : Controller
    {
        [HttpGet("/")]
        public IActionResult Root()
        {
            return View();
        }

        [HttpGet("/home")]
        public IActionResult Index()
        {
            ViewData["Products"] = Database.LocalDatabase.SelectProducts();
            return View();
        }

        [HttpPost("/add")]
        public IActionResult Index(string name, string description, string unit, string id)
        {
            Database.LocalDatabase.InsertProduct(new Models.Product
            {
                ProductId = long.Parse(id),
                Name = name,
                Description = description,
                Unit = unit
            });

            return Content("ok");
        }
    }
}
