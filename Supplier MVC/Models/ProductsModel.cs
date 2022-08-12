using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace Supplier_MVC.Models
{
    public class ProductsModel
    {
        [Key]
        public int ProductId { get; set; }

        [MaxLength(50)] public string Name { get; set; }

        [MaxLength(200)] public string Description { get; set; }

        public int Qty { get; set; }

        [MaxLength(50)] public string Unit { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public byte[]? Thumbnail { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}