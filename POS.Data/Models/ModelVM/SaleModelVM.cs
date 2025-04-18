using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM
{
    public class SaleModelVM
    {
        public int SaleID { get; set; }
        public string? SaleNumber { get; set; }
        public int TypeDocumentSaleID { get; set; }
        public int UserID { get; set; }
        public string CustomerDocument { get; set; }
        public string ClientName { get; set; }
        public int SubTotal { get; set; }
        public float Total { get; set; }
        public float TotalTaxes { get; set; }
    }
}
