using POS.Data.Models.ModelVM.Request;
using POS.Data.Models.ModelVM.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface ISalesReportRepository
    {
        Task<IEnumerable<SalesReportResponse>?> GetSalesReport(SalesReportRequest request);
    }
}
