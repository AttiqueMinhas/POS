using Microsoft.AspNetCore.Mvc;
using POS.Data.Models;
using POS.Data.Services.Definition;
using System.Security.Claims;

namespace POS.UI.Utilities.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;

        public MenuViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roleClaims = HttpContext.User?.FindAll(ClaimTypes.Role);

            if (roleClaims == null || !roleClaims.Any())
                return View(new List<MenuItem>());

            // Get menu items for all roles and merge them
            var menuItems = new List<MenuItem>();
            foreach (var roleClaim in roleClaims)
            {
                var items = await _menuService.GetMenuItemsForUserAsync(roleClaim.Value);
                menuItems.AddRange(items);
            }

            // Remove duplicates and maintain hierarchy
            menuItems = menuItems.GroupBy(x => x.MenuItemId)
                                .Select(g => g.First())
                                .ToList();

            return View(menuItems);
        }
    }
}
