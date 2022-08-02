using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Supplier_MVC.Context;

namespace Supplier_MVC.Controllers
{
    public class Registration : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext databaseContext;

        public Registration(UserManager<IdentityUser> userManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            this.databaseContext = databaseContext;
        }

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
            await _userManager.CreateAsync(new IdentityUser()
            {
                UserName = name
            }, password);

            await databaseContext.SaveChangesAsync();

            return RedirectPermanent("./login?RegisterSuccess=1");
        }
    }
}
