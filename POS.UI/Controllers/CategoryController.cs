using Microsoft.AspNetCore.Mvc;
using POS.Data.Models;
using POS.Data.Repositories.Definition;

namespace POS.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoriesRepository _category;

        public CategoryController(ICategoriesRepository category)
        {
            _category = category;
        }
        public async Task<IActionResult> Categories()
        {
            var categories = await _category.GetCategories();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> AddCategory(int id)
        {

            var cModel =  await _category.getCategoryById(id);
            return View(cModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryModel cModel)
        {
            if(ModelState.IsValid) {
                CategoryModel categoryModel = new CategoryModel();
                categoryModel.TotalRowCount = await _category.AddCategory(cModel);
                if (categoryModel.TotalRowCount > 0)
                {
                    return RedirectToAction(nameof(Categories));
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View();
            }
            
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            if(id > 0)
            {
                //CategoryModel cModel = new CategoryModel();
                var cModel =  await _category.getCategoryById(id);
                if(cModel != null)
                {
                    int TotalRowCount = await _category.DeleteCategory(id);
                    if(TotalRowCount > 0)
                    {
                        return RedirectToAction(nameof(Categories));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}
