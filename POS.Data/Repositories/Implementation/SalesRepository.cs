using Dapper;
using iTextSharp.text.pdf.qrcode;
using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Models.ModelVM;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Models.ModelVM.Response;
using POS.Data.Repositories.Definition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Implementation
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ISQLDataAccess _db;
        public SalesRepository(ISQLDataAccess db)
        {
            _db = db;
        }

        //public async Task<IEnumerable<ProductModel>> SearchProducts(string query)
        // {
        //    var sql = @"
        //        SELECT Product_Id, Product_Name, Price, Brand, Description
        //        FROM Product 
        //        WHERE Product_Name LIKE @Query 
        //        ORDER BY Product_Name";

        //    var parameters = new { Query = $"%{query}%" };

        //    //var products = await _db.QueryAsync(sql, parameters);
        //    var productList = await _db.GetQueryData<ProductModel, dynamic>(sql, parameters);
        //    return productList;
        //}

        public async Task<string> AddNewSale(SaleRequest request, int userId)
        {
            string spName = "sp_AddNewSale";
            // Convert productList to DataTable
            var productTable = new DataTable();
            productTable.Columns.Add("ProductID", typeof(int));
            productTable.Columns.Add("ProductBrand", typeof(string));
            productTable.Columns.Add("ProductName", typeof(string));
            productTable.Columns.Add("ProductCategory", typeof(string));
            productTable.Columns.Add("Quantity", typeof(int));
            productTable.Columns.Add("Price", typeof(decimal));
            productTable.Columns.Add("Total", typeof(decimal));

            foreach (var product in request.productList)
            {
                productTable.Rows.Add(product.ProductID, product.ProductBrand, product.ProductName, product.ProductCategory, product.Quantity, product.Price, product.Total);
            }

            // Define parameters for the stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("customerDocument", request.sale.CustomerDocument);
            parameters.Add("clientName", request.sale.ClientName);
            parameters.Add("typeDocumentSaleID", request.sale.TypeDocumentSaleID);
            parameters.Add("subTotal", request.sale.SubTotal);
            parameters.Add("totalTaxes", request.sale.TotalTaxes);
            parameters.Add("total", request.sale.Total);
            parameters.Add("userID", userId); // Add UserID to parameters
            parameters.Add("Details", productTable.AsTableValuedParameter("DetailSaleType")); // Pass TVP

            var saleNumber = await _db.GetScalarStringValue(spName, parameters);
            return saleNumber;
        }

        public async Task<SaleHistoryResponse> SaleHistory(SaleHistoryRequest request)
        {
            SaleHistoryResponse response = new SaleHistoryResponse();
            try
            {
                string spName = "sp_GetSalesHistory";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(request.SaleNumber))
                {
                    parameters.Add("saleNumber", request.SaleNumber);
                }
                else if (request.StartDate != null && request.EndDate != null)
                {
                    parameters.Add("startDate", request.StartDate);
                    parameters.Add("endDate", request.EndDate);
                }
                else if (request.StartDate != null)
                {
                    parameters.Add("startDate", request.StartDate);
                }
                else if (request.EndDate != null)
                {
                    parameters.Add("endDate", request.EndDate);
                }

                var result = await _db.GetQueryTableData<SaleModel, dynamic>(spName, parameters);

                if (!string.IsNullOrEmpty(request.SaleNumber))
                {
                    response.sale = result.FirstOrDefault(); // Suspected line
                }
                else
                {
                    response.sales = result.ToList();
                }

                return response;
            }
            catch (Exception ex)
            {
                // Log the error or return it in a custom response for debugging
                throw new Exception("Error in SaleHistory: " + ex.Message, ex);
            }
        }

        public async Task<SaleHistoryResponse> GetSaleBySaleNumber(SaleHistoryRequest request)
        {
            SaleHistoryResponse response = new SaleHistoryResponse();
            try
            {
                string spName = "sp_GetSaleBySaleNumber";
                var parameters = new DynamicParameters();
                parameters.Add("saleNumber", request.SaleNumber);
                
                var result = await _db.QueryMultipleAsync(spName, parameters);

                // Reading the first result set as SaleModel
                response.sale = result.Read<SaleModel>().FirstOrDefault();

                // Reading the second result set as a list of DetailSaleModel
                response.products = result.Read<DetailSaleModel>().ToList();

                return response;
            }
            catch (Exception ex)
            {
                // Log the error or return it in a custom response for debugging
                throw new Exception("Error in SaleHistory: " + ex.Message, ex);
            }
        }

        public async Task<List<ProductRecommendationModel>> getRecommendedProducts(ProductRecommendationRequest request)
        {
            try
            {
                string spName = "sp_GetRecommendedProducts";
                // Log the parameter for debugging
                //Console.WriteLine($"Parameter being sent: @pId = {request.ProductId}");
                var parameters = new DynamicParameters();
                parameters.Add("pId", request.ProductId);  // Fixed: Added '@'

                var response = await _db.GetDataListFromSP<ProductRecommendationModel, dynamic>(spName, parameters);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getRecommendedProducts: " + ex.Message, ex);
            }

        }

    }
}
