using Dapper;
using POS.Data.DataAccess;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Models.ModelVM.Response;
using POS.Data.Repositories.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Implementation
{
    public class SalesReportRepository : ISalesReportRepository
    {
        private readonly ISQLDataAccess _db;
        public SalesReportRepository(ISQLDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SalesReportResponse>?> GetSalesReport(SalesReportRequest request)
        {
            var spName = "sp_GetSaleReports";
            var parameters = new DynamicParameters();
            parameters.Add("@startDate", request.StartDate);
            parameters.Add("@endDate", request.EndDate);
            var response = await _db.GetData<SalesReportResponse, dynamic>(spName, parameters);
            return response;
        }
    }
}
