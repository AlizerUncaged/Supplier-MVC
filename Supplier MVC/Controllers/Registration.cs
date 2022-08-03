using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Supplier_MVC.Context;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Supplier_MVC.Controllers
{
    [AllowAnonymous]
    public class Registration : Controller
    {
        private readonly UserManager<Models.SupplierUser> _userManager;
        private readonly DatabaseContext databaseContext;

        public Registration(UserManager<Models.SupplierUser> userManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            this.databaseContext = databaseContext;
        }

        [HttpGet("/registration")]
        public async Task<IActionResult> Index(string error = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
                ViewData["IsError"] = error;

            return View();
        }

        [HttpPost("/registration")]
        public async Task<IActionResult> RegisterAccount(
            string name,
            string address,
            string representative,
            string number,
            string password)
        {
            var registerResult = await _userManager.CreateAsync(new Models.SupplierUser()
            {
                UserName = name,
                PasswordHash = password,
            }, password);

            if (!registerResult.Succeeded) return RedirectPermanent($"./registration?error=Register failed. {string.Join(", ", registerResult.Errors.Select(x => x.Description))}");

            await databaseContext.Suppliers.AddAsync(new Models.SupplierModel
            {
                Address = address,
                CompanyName = name,
                ContactNo = number,
                DateAdded = System.DateTime.Now,
                DateModified = System.DateTime.Now,
                Representative = representative,
                SupplierId = databaseContext.Suppliers.Count()
            });

            await databaseContext.SaveChangesAsync();

            return RedirectPermanent("./login?RegisterSuccess=1");
        }
    }
}
