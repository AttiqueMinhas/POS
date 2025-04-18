using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    public class SaleReceiptModel
    {
        public string SaleNumber { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ClientName { get; set; }
        public string ClientDocument { get; set; }
        public string SubTotal { get; set; }
        public string Taxes { get; set; }
        public string Total { get; set; }
        public List<ProdModel> Products { get; set; }
    }
}
