using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Repositories.Definition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Implementation
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ISQLDataAccess _db;
        public MenuRepository(ISQLDataAccess db)
        {
            _db = db;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            const string sql = @"SELECT * FROM Roles WHERE RoleName = @RoleName AND IsActive = 1";
            var result = await _db.GetQueryData<Role, dynamic>(sql, new { RoleName = roleName });
            return result.FirstOrDefault();
        }

        //public async Task<List<MenuItem>> GetMenuItemsByRoleAsync(int roleId)
        //{
        //    const string sql = @"
        //    SELECT m.* 
        //    FROM MenuItems m
        //    INNER JOIN RoleMenu rm ON m.MenuItemId = rm.MenuID
        //    WHERE rm.RoleId = @RoleId 
        //    AND rm.IsActive = 1 
        //    AND m.IsActive = 1
        //    ORDER BY m.DisplayOrder";
        //    var menuItems = await _db.GetDataList<MenuItem, dynamic>(sql, new { RoleId = roleId });
        //    return BuildMenuHierarchy(menuItems);
        //}

        public async Task<List<MenuItem>> GetMenuItemsByRoleAsync(int roleId)
        {
            // First get all menu items directly assigned to the role
            const string directItemsSql = @"
                SELECT m.* 
                FROM MenuItems m
                INNER JOIN RoleMenu rm ON m.MenuItemId = rm.MenuID
                WHERE rm.RoleId = @RoleId 
                AND rm.IsActive = 1 
                AND m.IsActive = 1";

            var directItems = await _db.GetDataList<MenuItem, dynamic>(directItemsSql, new { RoleId = roleId });

            if (!directItems.Any())
                return new List<MenuItem>();

            // Get IDs of all direct items and their parents
            var allMenuItemIds = directItems
                .Select(m => m.MenuItemId)
                .Concat(directItems.Where(m => m.ParentMenuItemId.HasValue)
                                  .Select(m => m.ParentMenuItemId.Value))
                .Distinct()
                .ToList();

            // Now get all needed menu items (both direct and their parents)
            const string allItemsSql = @"
                SELECT * FROM MenuItems 
                WHERE MenuItemId IN @MenuItemIds 
                AND IsActive = 1
                ORDER BY DisplayOrder";

            var allItems = await _db.GetDataList<MenuItem, dynamic>(allItemsSql, new { MenuItemIds = allMenuItemIds });

            return BuildMenuHierarchy(allItems);
        }
        private List<MenuItem> BuildMenuHierarchy(List<MenuItem> allItems)
        {
            var rootItems = allItems.Where(m => m.ParentMenuItemId == null).ToList();

            foreach (var item in rootItems)
            {
                item.Children = allItems.Where(m => m.ParentMenuItemId == item.MenuItemId).ToList();
            }

            return rootItems;
        }
    }
}
