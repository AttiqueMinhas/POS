using POS.Data.Models;
using POS.Data.Repositories.Definition;
using POS.Data.Repositories.Implementation;
using POS.Data.Services.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Services.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repo;
        public MenuService(IMenuRepository menuRepository)
        {
            _repo = menuRepository;
        }

        public async Task<List<MenuItem>> GetMenuItemsForUserAsync(string roleName)
        {
            var role = await _repo.GetRoleByNameAsync(roleName);
            if (role == null) return new List<MenuItem>();

            return await _repo.GetMenuItemsByRoleAsync(role.RoleId);
        }
    }
}
