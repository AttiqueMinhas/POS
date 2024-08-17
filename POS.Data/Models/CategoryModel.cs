using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    public class CategoryModel
    {
        public int Category_ID { get; set; }
        [Required(ErrorMessage ="Please Enter the name of Category")]
        public string Category_Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Status { get; set; }
        public int TotalRowCount { get; set; }
    }
}
