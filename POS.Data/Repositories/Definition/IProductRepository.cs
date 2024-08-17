using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> ProductList();
        Task<int> AddProduct(ProductModel pmodel);

        Task<ProductModel> GetProductById(int productId);
        Task<int> DeleteProduct(int Id);

    }
}
