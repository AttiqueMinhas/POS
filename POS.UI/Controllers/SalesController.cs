using Microsoft.AspNetCore.Mvc;

namespace POS.UI.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult NewSale()
        {
            return View();
        }
        public IActionResult SalesHistory()
        {
            return View();
        }
    }
}
