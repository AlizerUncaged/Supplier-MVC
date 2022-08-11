using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Supplier_MVC.Context;
using Supplier_MVC.Models;

namespace Supplier_MVC.Controllers
{
    public class SupplierOptions : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<SupplierUser> _userManager;

        public SupplierOptions(DatabaseContext databaseContext, UserManager<SupplierUser> userManager)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("/dashboard")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [Authorize]
        [HttpGet("/purchased")]
        public async Task<IActionResult> PurchaseData()
        {
            ViewData["Products"] = _databaseContext.PurchaseOrderDetails.Select(x => new PurchaseTableModel()
            {
                Id = x.Id, Added = x.PurchaseOrderHeaders.DateAdded,
                Amount = x.Amount,
                Price = x.Price,
                Qty = x.Qty,
                Received = x.PurchaseOrderHeaders.DateReceived,
                Status = x.PurchaseOrderHeaders.Status,
                Product = _databaseContext.Products.FirstOrDefault(j=>j.ProductId == x.ProductId)!.Name,
                Supplier = _databaseContext.Suppliers.FirstOrDefault(y =>
                    y.SupplierId == x.PurchaseOrderHeaders.SupplierId)!.CompanyName!
            }).ToList();

            ViewData["AvailableProducts"] = _databaseContext.Products.ToList();
            return View();
        }

        [Authorize]
        [HttpPost("/changeStatus")]
        public async Task<IActionResult> ChangePurchaseStatus(string status, int purchaseId)
        {
            var purchaseRecord = _databaseContext.PurchaseOrderDetails.FirstOrDefault(x => x.Id == purchaseId);


            var purchaseOrderHeader =
                _databaseContext.PurchaseOrderHeaders.FirstOrDefault(x => x.Id == purchaseRecord.Id);

            purchaseOrderHeader.Status = status;
            await _databaseContext.SaveChangesAsync();


            return Content("Status Changed");
        }

        [Authorize]
        [HttpPost("/removePurchase")]
        public async Task<IActionResult> RemovePurchase(int delPurchaseId)
        {
            var foundPurchase = _databaseContext.PurchaseOrderDetails.FirstOrDefault(x => x.Id == delPurchaseId);
            if (foundPurchase is { })
                _databaseContext.PurchaseOrderDetails.Remove(foundPurchase);

            await _databaseContext.SaveChangesAsync();
            return Content("Removed");
        }


        [Authorize]
        [HttpPost("/addPurchased")]
        public async Task<IActionResult> AddPurchased(int prodId, double amount, int qty, double price)
        {
            if (!_databaseContext.Products.Any(x => x.ProductId == prodId))
            {
                ViewData["IsError"] = "Product does not exist.";
                return Content("Error");
            }

            var tUser = await _userManager.GetUserAsync(HttpContext.User);
            var supplier = _databaseContext.Suppliers.FirstOrDefault(x => x.CompanyName == tUser.SupplierName);
            var product = _databaseContext.Products.Where(x => x.ProductId == prodId);
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
                ProductId = prodId, Amount = amount, Qty = qty, Price = price,
                PurchaseOrderHeaders = purchaseHeader.Entity, PurchaseOrderHeaderId = purchaseHeader.Entity.Id
            });

            await _databaseContext.SaveChangesAsync();
            return Content("Added");
        }
    }
}