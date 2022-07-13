using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier_MVC.Controllers
{
    public class Login : Controller
    {
        [HttpGet("/login")]
        public async Task<IActionResult> Index()
        {
            if (Request.Query.ContainsKey("RegisterSuccess"))
                ViewData["RegisterSuccess"] = 1;

            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LoginAttempt(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return Content("Account not found.");

            var accounts = Database.LocalDatabase.SelectSuppliersMetadata();
            var account = accounts.Where(x => x is { } && x.Name is { } && x.Password is { } && x.Name.ToLower() == username.ToLower() && x.Password == password);

            if (!account.Any())
                return Content("Account not found.");

            var acc = account.First();

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);

            Response.Cookies.Append("username", acc.Name, option);
            Response.Cookies.Append("password", acc.Password, option);

            return RedirectPermanent("./home");
        }
    }
}
