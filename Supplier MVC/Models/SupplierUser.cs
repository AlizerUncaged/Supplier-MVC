using Microsoft.AspNetCore.Identity;

namespace Supplier_MVC.Models
{
    public class SupplierUser : IdentityUser
    {
        public string? SupplierName { get; set; }
    }
}
