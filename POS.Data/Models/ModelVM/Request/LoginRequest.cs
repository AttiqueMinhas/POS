using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM.Request
{
    public class LoginRequest
    {
        [Required,Display(Name ="Username or Email")]
        [EmailAddress]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
