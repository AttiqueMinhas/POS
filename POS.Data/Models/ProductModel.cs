using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    public class ProductModel
    {
        public int Product_Id { get; set; }
        public int Category_ID { get; set; }
        [Required]
        public string? Category_Name {  get; set; }
        [Required]
        public string Product_Name { get; set; }
        [Required]
        public int BarCode { get; set; }
        [Required]
        public string Brand { get; set; }

        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public bool Status { get; set;}
        public int totalRowsCount { get; set; }
        public IFormFile Image { get; set; }
        public string IPath { get; set; }

        private IEnumerable<CategoryModel> _category = new List<CategoryModel>();
        public IEnumerable<CategoryModel> Categories { get { return _category; } set { _category = value; } }
    }
}
