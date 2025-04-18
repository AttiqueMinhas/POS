using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM.Response
{
    public class SaleHistoryResponse
    {
        public SaleModel? sale { get; set; }
        public List<SaleModel>? sales { get; set; }
        public List<DetailSaleModel>? products { get; set; }
    }
}
