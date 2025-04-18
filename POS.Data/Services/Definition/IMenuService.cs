using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Services.Definition
{
    public interface IMenuService
    {
        Task<List<MenuItem>> GetMenuItemsForUserAsync(string roleName);
    }
}
