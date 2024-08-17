using Microsoft.AspNetCore.Mvc;

namespace POS.UI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
