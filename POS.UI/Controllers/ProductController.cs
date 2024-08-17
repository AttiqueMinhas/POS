using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Repositories.Definition;

namespace POS.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _pservice;
        private readonly ICategoriesRepository _cservice;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository pservice, ICategoriesRepository cservice, IWebHostEnvironment webHostEnvironment)
        {
            _pservice = pservice;
            _cservice = cservice;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task<IActionResult> Products()
        {
            ProductModel productModel = new ProductModel();
            var pmodel = await _pservice.ProductList();
            //_cservice.getCategoryById(productModel.Category_ID);
            return View(pmodel);
        }

        public async Task<IActionResult> AddEditProduct(int id)
        {
            ProductModel productModel = new ProductModel();
            if (id == 0)
            {
                productModel.Categories = await _cservice.GetCategories();
                return View(productModel);
            }
            else
            {
                //ProductModel pModel = new ProductModel();
                var product = await _pservice.GetProductById(id);
                productModel.Categories = await _cservice.GetCategories();
                productModel.Category_ID = product.Category_ID; 
                productModel.Image = product.Image;
                productModel.Product_Name = product.Product_Name;
                productModel.BarCode = product.BarCode;
                productModel.Price = product.Price;
                productModel.Brand = product.Brand;
                productModel.Description = product.Description;
                productModel.Quantity = product.Quantity;
                productModel.Status = product.Status;
                return View(productModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEditProduct(ProductModel pmodel)
        {
            ProductModel model = new ProductModel();
            //model.Categories = await _cservice.GetCategories();

            //_pservice.
            if(pmodel.Image != null)
            {
                if(pmodel.Image.Length <= 1048576)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + pmodel.Image.FileName;
                    var imageFolderPath = "image/products/";
                    pmodel.IPath = "/" + imageFolderPath + uniqueFileName;

                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolderPath);

                    if (!Directory.Exists(serverFolder))
                    {
                        Directory.CreateDirectory(serverFolder);
                    }

                    await pmodel.Image.CopyToAsync(new FileStream(Path.Combine(serverFolder, uniqueFileName), FileMode.Create));

                }
                else
                {
                    ViewBag.img = "Upload Image file less than 10 mb";
                }

                var TotalrowsCount = await _pservice.AddProduct(pmodel);
                if(TotalrowsCount > 0)
                { 
                    return RedirectToAction(nameof(Products));
                }
            }

            return View();
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productModel = await _pservice.GetProductById(id);
            if(productModel != null)
            {
                int TotalRowsCount = await _pservice.DeleteProduct(id);
                if(TotalRowsCount > 0)
                {
                    return RedirectToAction(nameof(Products));
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
