using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Supplier_MVC.Context;
using Supplier_MVC.Models;

namespace Supplier_MVC.Controllers
{
    public class ShoppingView : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<SupplierUser> _userManager;

        public ShoppingView(DatabaseContext databaseContext, UserManager<SupplierUser> userManager)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }

        [HttpGet("/shopping")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("/shopping/items/{productId}")]
        public async Task<IActionResult> ViewItem(int productId)
        {
            ViewBag.Product = _databaseContext.Products.FirstOrDefault(x => x.ProductId == productId)!;
            return View();
        }

        [HttpPost("/ShoppingView/Buy")]
        public async Task<IActionResult> Buy(BuyModel buyModel)
        {
            var product = _databaseContext.Products.FirstOrDefault(x => x.ProductId == buyModel.ProductId);

            if (product is null) return Content("Product does not exist.");

            buyModel.Qty = buyModel.Qty > product.Qty ? product.Qty : buyModel.Qty;
            var tUser = await _userManager.GetUserAsync(HttpContext.User);
            var supplier = _databaseContext.Suppliers.FirstOrDefault(x => x.CompanyName == tUser.SupplierName);
            // Add PurchaseOrder header.
            var purchaseHeader = await _databaseContext.PurchaseOrderHeaders.AddAsync(new PurchaseOrderHeadersModel()
            {
                SupplierId = supplier.SupplierId,
                DateAdded = DateTime.Now,
                Date = DateTime.Now,
                Status = "Created"
            });

            // Add purchase details model.
            await _databaseContext.PurchaseOrderDetails.AddAsync(new PurchaseOrderDetailsModel()
            {
                ProductId = product.ProductId, Amount = buyModel.Qty * product.Price, Qty = buyModel.Qty,
                Price = product.Price, PaymentType = buyModel.PaymentType,
                PurchaseOrderHeaders = purchaseHeader.Entity, PurchaseOrderHeaderId = purchaseHeader.Entity.Id
            });

            product.Qty -= buyModel.Qty;

            await _databaseContext.SaveChangesAsync();

            return Content("Success");
        }
    }
}