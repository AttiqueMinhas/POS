using Microsoft.AspNetCore.Mvc;
using POS.Data.Repositories.Definition;

namespace POS.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _repo;
        public DashboardController(IDashboardRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var dashboardData = await _repo.GetDashboardDataAsync();
                return View(dashboardData);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new
                {
                    Error = "An error occurred while fetching dashboard data",
                    Details = ex.Message
                });
            }
        }
    }
}
