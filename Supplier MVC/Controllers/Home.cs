using Microsoft.AspNetCore.Mvc;

namespace Supplier_MVC.Controllers
{
    public class Home : Controller
    {
        [HttpGet("/")]
        public IActionResult Root()
        {
            Response.Redirect("./login");
            return Content(string.Empty);
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

        [HttpPost("/remove")]
        public IActionResult Index(string id)
        {
            Database.LocalDatabase.RemoveProduct(new Models.Product
            {
                ProductId = long.Parse(id),
            });

            return Content("ok");
        }
    }
}
