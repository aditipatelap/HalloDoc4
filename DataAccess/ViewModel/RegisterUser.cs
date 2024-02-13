using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter Your Email Address.")]
        public string? Email { get; set; }
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one digit")]
        //[StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters long")]

        [Required(ErrorMessage = "Please enter Your Password.")]
        public string? Password { get; set; }
    }

}
