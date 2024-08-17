using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    enum roleId
    {
        Admin = 4,
        Employee = 5,
        Inspector = 6
    }
    public class UserModel
    {
        public int? UserID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{5,14}$",ErrorMessage = "Username must have at least 6 characters and maximum 15 characters")]
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
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_])[A-Za-z][A-Za-z\d\W_]{7,29}$", ErrorMessage = "Password should start with alphabet and 1 uppercase, 1 lowercase letter,1 numeric number, 1 special character and min 8 character and max 30 characters long")]
        public string Password { get; set; }
        [Required]
        public bool State { get; set; }
        [Required]
        public IFormFile Image { get; set; }

        public string? ImagePath { get; set; }

        public byte[]? Salt { get; set; }
    }
}
