using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Repositories.Definition;


namespace POS.Data.Repositories.Implementation
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ISQLDataAccess _db;

        public CategoriesRepository(ISQLDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CategoryModel>> GetCategories()
        {
            string spName = "sp_GetCategories";
            return await _db.GetData<CategoryModel, dynamic>(spName, new { });
        }

        public async Task<int> AddCategory(CategoryModel cModel)
        {
            string spName = "CategoryAddEdit";
            int TotalRowsCount = await _db.SaveData(spName, new
            {
                cModel.Category_ID,
                cModel.Category_Name,
                cModel.Description,
                cModel.Status
            });
            return TotalRowsCount;
        }

        //public async Task<int> UpdateCategory(CategoryModel cModel)
        //{
        //    string spName = "CategoryAddEdit";
        //    int to
        //}
        public async Task<CategoryModel?> getCategoryById(int id)
        {
            string spName = "sp_GetCategoryById";
            var category = await _db.GetData<CategoryModel, dynamic>(spName, new { Id = id});
            return category.FirstOrDefault();
        }

        public async Task<int> DeleteCategory(int id) 
        {
            string spName = "sp_CategoryDelete";
           
           int TotalRowCount = await _db.SaveData(spName, new { Category_Id = id });
           return TotalRowCount;
        }

    }
}
