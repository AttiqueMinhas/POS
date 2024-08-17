using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<CategoryModel>> GetCategories();
        Task<int> AddCategory(CategoryModel cModel);
        Task<CategoryModel?> getCategoryById(int id);
        Task<int> DeleteCategory(int id);
    }
}
