using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    public class SaleModel
    {
        public int SaleID { get; set; }
        public string? SaleNumber { get; set; }
        [Required(ErrorMessage = "Type is a required field.")]
        public int TypeDocumentSaleID { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "client id is a required field.")]
        public string CustomerDocument { get; set; }
        [Required(ErrorMessage = "Full Name is a required field.")]
        public string ClientName { get; set; }
        [Required]
        public int SubTotal { get; set; }
        [Required]
        public float Total { get; set; }
        [Required]
        public float TotalTaxes { get; set; }
        public string? DocumentType { get; set; }
        public DateTime? CreatedAT { get; set; }
        public string? RegisterUser { get; set; }
    }

}
