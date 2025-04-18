using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM.Request
{
    public class SaleRequest
    {
        public SaleModel sale { get; set; }
        public IEnumerable<SelectListItem>? documentTypes { get; set; }
        public List<DetailSaleModel> productList { get; set; }
    }
}
