﻿using Microsoft.AspNetCore.Mvc;

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
            var accounts = Database.LocalDatabase.SelectSuppliersMetadata();
            var account = accounts.Where(x => x.Name.ToLower() == username.ToLower() && x.Password == password);

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
