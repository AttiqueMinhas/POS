using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM.Response
{
    public class SalesReportResponse
    {
        public string? SaleNumber { get; set; }
        public int? TypeDocumentSaleID { get; set; }
        public string? ClientDocument { get; set; }
        public string? ClientName { get; set; }
        public int SubTotalSale { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public float TaxTotalSale { get; set; }
        public float TotalSale { get; set; }
        public string? DocumentType { get; set; }
        public string? Product { get; set; }
        public int? Quantity { get; set; }
        public float? Price { get; set; }
        public float? Total { get; set; }

    }
}
