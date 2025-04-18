using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface IMenuRepository
    {
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<List<MenuItem>> GetMenuItemsByRoleAsync(int roleId);
    }
}
