using Azure;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Models.ModelVM.Response;
using POS.Data.Repositories.Definition;

namespace POS.UI.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ISalesReportRepository _repo;
        public ReportsController(ISalesReportRepository repo)
        {
            _repo  = repo;
        }
        [HttpGet]
        public IActionResult SalesReport()
        {
            var response = new List<SalesReportResponse>();
            return View(response);
        }

        //[HttpPost]
        //public async Task<IActionResult> GetSaleReportData([FromBody]SalesReportRequest request)
        //{
        //    var response = await _repo.GetSalesReport(request);
        //    return Json(response ?? new List<SalesReportResponse>());
        //}
        [HttpPost]
        public async Task<IActionResult> GetSaleReportData([FromForm]SalesReportRequest request)
        {
            var response = await _repo.GetSalesReport(request);
            return View("SalesReport", response ?? new List<SalesReportResponse>());
        }
    }
}
