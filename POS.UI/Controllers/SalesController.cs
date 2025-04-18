using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using POS.Data.Models;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Models.ModelVM.Response;
using POS.Data.Repositories.Definition;
using POS.UI.Services;

//using System.Web.Mvc;
using System.Xml.Linq;

namespace POS.UI.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesRepository _repo;
        private readonly IProductRepository _productRepo;
        private readonly ITypeDocumentSaleRepository _repoTypeDocSale;
        public SalesController(ISalesRepository repo, ITypeDocumentSaleRepository repoTypeDocSale, IProductRepository productRepo)
        {
            _repo = repo;
            _repoTypeDocSale = repoTypeDocSale;
            _productRepo = productRepo;
        }
        public async Task<IActionResult> NewSale()
        {
            var documentTypes = await _repoTypeDocSale.TypeDocumentSaleList();

            var documentTypeList = new SelectList(documentTypes, "TypeDocumentSaleID", "Name");
            var saleRequest = new SaleRequest
            {
                sale = new SaleModel(),
                documentTypes = documentTypeList
            };
            return View(saleRequest);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewSale([FromBody] SaleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }
            try
            {
                // Retrieve the logged-in user's ID from claims
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID");
                if (userIdClaim == null)
                {
                    return Unauthorized("User not logged in.");
                }

                int userId = int.Parse(userIdClaim.Value);

                // Pass the user ID to the repository
                var saleNumber = await _repo.AddNewSale(request, userId);
                return Json(new { SaleNumber = saleNumber }); // Return the SaleNumber
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error in AddNewSale: " + ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public IActionResult SalesHistory()
        {
            return View();
        }

        public async Task<IActionResult> SaleHistory([FromBody]SaleHistoryRequest request)
        {
            SaleHistoryResponse response = new SaleHistoryResponse();
            response = await _repo.SaleHistory(request);
            return Json(response);
        }

        public async Task<IActionResult> GetSaleBySaleNumber([FromBody] SaleHistoryRequest request)
        {
            SaleHistoryResponse response = new SaleHistoryResponse();
            response = await _repo.GetSaleBySaleNumber(request);
            return Json(response);
        }

        public async Task<IActionResult> SearchProducts(string query)
        {
            var response = await _productRepo.SearchProducts(query);
            return Json(response);
        }

        [HttpPost]
        public IActionResult GenerateReceiptPdf([FromBody] SaleReceiptModel model)
        {
            var invoiceService = new InvoiceService();
            var pdf = invoiceService.GenerateSaleReceipt(
                model.SaleNumber,
                model.StoreName,
                model.Address,
                model.Email,
                model.ClientName,
                model.ClientDocument,
                model.Products.Select(p => new ProductItem
                {
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Total = p.Total
                }).ToList(),
                model.SubTotal,
                model.Taxes,
                model.Total
            );

            using (var stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                return File(stream.ToArray(), "application/pdf");
            }
        }

        //[HttpGet]
        //public IActionResult PrintSale(string saleNumber)
        //{
        //    // Fetch sale details by saleNumber from DB
        //    var sale = _saleService.GetSaleDetails(saleNumber); // however you fetch it

        //    // Generate PDF using any library (example: iTextSharp or others)
        //    byte[] pdfBytes = _pdfService.GenerateSalePdf(sale);

        //    return File(pdfBytes, "application/pdf", $"Sale_{saleNumber}.pdf");
        //}

    }
}
