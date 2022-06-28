using Microsoft.AspNetCore.Mvc;

namespace Supplier_MVC.Controllers
{
    public class Registration : Controller
    {
        [HttpGet("/registration")]
        public async Task<IActionResult> Index() => View();

        [HttpPost("/registration")]
        public async Task<IActionResult> RegisterAccount(
            string name,
            string address,
            string representative,
            string number,
            string password)
        {

            Database.LocalDatabase.InsertSupplier(name, address, representative, number, password);

            return RedirectPermanent("./login?RegisterSuccess=1");
        }
    }
}
