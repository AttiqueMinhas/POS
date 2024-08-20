using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM.Request
{
    public class EditRequestVM
    {
        public int? UserID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{5,14}$", ErrorMessage = "Username must have at least 6 characters and maximum 15 characters")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.\w+$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Role { get; set; }
        public int? RoleID { get; set; }
        [Required]
        public bool State { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
        public string? ExistingImagePath { get; set; }

    }
}
