using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM
{
    public class DetailSaleModel
    {
        public int? DetailSaleID { get; set; }
        public int? SaleID { get; set; }
        public int ProductID { get; set; }
        [Required]
        public string? ProductCategory { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductBrand { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Total { get; set; }
    }
}
