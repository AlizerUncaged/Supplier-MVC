using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Supplier_MVC.Controllers
{
    public class ShoppingView : Controller
    {
        [HttpGet("/shopping")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}