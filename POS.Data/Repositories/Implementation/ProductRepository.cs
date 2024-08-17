using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Repositories.Definition;


namespace POS.Data.Repositories.Implementation
{
    public class ProductRepository:IProductRepository
    {
        private readonly ISQLDataAccess _db;
        public ProductRepository(ISQLDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProductModel>> ProductList()
        {
            string spName = "sp_ProductListWithCategoryName";
            return await _db.GetData<ProductModel,dynamic>(spName, new { });
        }

        public async Task<int> AddProduct(ProductModel pmodel)
        {
            ProductModel model = new ProductModel();
            string spName = "sp_ProductAddEdit";
          var totalRowsCount = await _db.SaveData<dynamic>(spName, new
            {
                pmodel.Product_Id,
                pmodel.Product_Name,
                pmodel.Category_ID,
                pmodel.BarCode,
                pmodel.Brand,
                pmodel.Description,
                pmodel.Quantity,
                pmodel.Price,
                pmodel.Status,
                //pmodel.Categories,
                pmodel.IPath
            });

            return totalRowsCount;
        }

        public async Task<ProductModel> GetProductById(int Id)
        {
            string spName = "sp_GetProductById";
            
            var productModel = await _db.GetData<ProductModel,dynamic>(spName, new { 
                 Id
            });
            return productModel.FirstOrDefault();
        }


        public async Task<int> DeleteProduct(int Id)
        {
            string spName = "sp_deleteProduct";
            var TotalRowsCount = await _db.SaveData(spName, new { Id });
            return TotalRowsCount;

        }
    }
}
