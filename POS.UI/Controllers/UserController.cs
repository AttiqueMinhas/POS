using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Repositories.Definition;
using System.Text.RegularExpressions;

namespace POS.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        private IWebHostEnvironment _webHostEnvironment;
        public UserController(IUserRepository userRepo, IWebHostEnvironment webHostEnvironment)
        {
            _userRepo = userRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Users()
        {
            List<UserModel> users = await _userRepo.getUsers();
            if(users != null)
            {
                return View(users);
            }
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> CheckEmail([FromBody] UserModel model)
        {
            ModelState.Remove("UserName");
            ModelState.Remove("Phone");
            ModelState.Remove("Role");
            ModelState.Remove("Password");
            ModelState.Remove("State");
            ModelState.Remove("Image");
            if (string.IsNullOrEmpty(model.Email))
            {
                return Json(new { isValid = false, message = "Email is a required field" });
            }

            
            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, message = "Email is not valid" });
            }

            bool result = await _userRepo.IsEmailExists(model.Email);
            if (result)
            {
                return Json(new { isValid = false, message = "Email already exists." });
            }
            else
            {
                return Json(new { isValid = true, message = "Email does not exist in the database." });
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetUserByID(int id)
        {
            if(id != 0)
            {
               var user = await _userRepo.getUserByID(id);
               if (user != null)
                {
                    return Json(new { isValid = true, userData = user});
                }
               else
                {
                    return Json(new { isValid = false, message = "User Not Found." });
                }
            }
            else
            {
                return Json(new { isValid = false, message = "Please Enter Id of the User." });
            }
        }

        [HttpPost]
        public async  Task<IActionResult> RegisterUser([FromForm]UserModel model)
        {
            if (!ModelState.IsValid)
            {
                // Create a dictionary to hold errors
                var errors = new Dictionary<string, string>();

                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        var key = state.Key;
                        var errorMessage = state.Value.Errors.FirstOrDefault()?.ErrorMessage;
                        if (key.EndsWith(".") && key.Length > 0)
                        {
                            key = key.Substring(0, key.Length - 1); // Remove trailing period
                        }
                        errors.Add(key + "Error", errorMessage);
                    }
                }

                return Json(errors);
            }
            else
            {
                if (model.Image != null)
                {
                    if (model.Image.Length <= 1048576)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Image.FileName);
                        var imageFolderPath = "image/users/";
                        model.ImagePath = "/" + imageFolderPath + uniqueFileName;

                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolderPath);

                        if (!Directory.Exists(serverFolder))
                        {
                            Directory.CreateDirectory(serverFolder);
                        }
                        using (var fileStream = new FileStream(Path.Combine(serverFolder, uniqueFileName), FileMode.Create))
                        {
                            await model.Image.CopyToAsync(fileStream);
                        }
                        //await model.Image.CopyToAsync(new FileStream(Path.Combine(serverFolder, uniqueFileName), FileMode.Create));
                    }
                    else
                    {
                        ViewBag.img = "Upload Image file less than 10 mb";
                    }

                    int TotalrowsCount = await _userRepo.AddNewUser(model);
                    if (TotalrowsCount > 0)
                    {
                        return Ok(new { isValid = true });
                    }
                    else
                    {
                        return Ok(new { isValid = false });
                    }
                }
                return Ok(new { isValid = true });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser([FromForm] EditRequestVM model)
        {
            if (!ModelState.IsValid)
            {
                // Create a dictionary to hold errors
                var errors = new Dictionary<string, string>();

                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        var key = state.Key;
                        var errorMessage = state.Value.Errors.FirstOrDefault()?.ErrorMessage;
                        if (key.EndsWith(".") && key.Length > 0)
                        {
                            key = key.Substring(0, key.Length - 1); // Remove trailing period
                        }
                        errors.Add(key + "Error", errorMessage);
                    }
                }

                return Json(errors);
            }
            else
            {
                if (model.Image != null)
                {
                    if (model.Image.Length <= 1048576)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Image.FileName);
                        var imageFolderPath = "image/users/";
                        model.ImagePath = "/" + imageFolderPath + uniqueFileName;

                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolderPath);

                        if (!Directory.Exists(serverFolder))
                        {
                            Directory.CreateDirectory(serverFolder);
                        }
                        using (var fileStream = new FileStream(Path.Combine(serverFolder, uniqueFileName), FileMode.Create))
                        {
                            await model.Image.CopyToAsync(fileStream);
                        }
                        //await model.Image.CopyToAsync(new FileStream(Path.Combine(serverFolder, uniqueFileName), FileMode.Create));
                    }
                    else
                    {
                        ViewBag.img = "Upload Image file less than 10 mb";
                    }

                    int TotalrowsCount = await _userRepo.editUser(model);

                    // Process the model if valid
                    if(TotalrowsCount > 0)
                    {
                        return Ok(new { isValid = true });
                    }
                    else
                    {
                        return Ok(new { isValid = false });
                    }
                    
                }
                else
                {
                    model.ImagePath = model.ExistingImagePath;
                    int TotalrowsCount = await _userRepo.editUser(model);
                    // Process the model if valid
                    if (TotalrowsCount > 0)
                    {
                        return Ok(new { isValid = true });
                    }
                    else
                    {
                        return Ok(new { isValid = false });
                    }
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id != 0)
            {
                var totalRowsCount = await _userRepo.deleteUser(id);
                if (totalRowsCount > 0)
                {
                    return Json(new { isValid = true, message = "User Deleted Successfully." });
                }
                else
                {
                    return Json(new { isValid = false, message = "User Not Found." });
                }
            }
            else
            {
                return Json(new { isValid = false, message = "Please Enter Id of the User." });
            }
        }
    }
}
