using POS.Data.Models;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Models.ModelVM.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface ISalesRepository
    {
        Task<string> AddNewSale(SaleRequest request, int userId);
        Task<SaleHistoryResponse> SaleHistory(SaleHistoryRequest request);
        Task<SaleHistoryResponse> GetSaleBySaleNumber(SaleHistoryRequest request);
        Task<List<ProductRecommendationModel>> getRecommendedProducts(ProductRecommendationRequest request);
        //Task<IEnumerable<ProductModel>> SearchProducts(string query);
    }
}
