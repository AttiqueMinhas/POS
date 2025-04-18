using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string MenuText { get; set; }
        public string MenuUrl { get; set; }
        public int? ParentMenuItemId { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string IconClass { get; set; }
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
