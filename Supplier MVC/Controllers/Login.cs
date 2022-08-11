using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier_MVC.Controllers
{
    [AllowAnonymous]
    public class Login : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.SignInManager<Models.SupplierUser> signInManager;

        public Login(SignInManager<Models.SupplierUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet("/debug")]
        public async Task<IActionResult> asdasdas()
        {
            return Content("hi");
        }

        [HttpGet("/login")]
        public async Task<IActionResult> Index(bool error = false, string ReturnUrl = null)
        {
            if (Request.Query.ContainsKey("RegisterSuccess"))
                ViewData["RegisterSuccess"] = 1;

            if (error)
                ViewData["IsError"] = "Invalid login.";

            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                ViewData["IsError"] = "You need to be logged in first.";

            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LoginAttempt(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(username, password, true, true);

                if (result.Succeeded)
                    return RedirectPermanent("./dashboard");
            }

            //IsError

            return RedirectPermanent("./login?error=true");
        }
    }
}
