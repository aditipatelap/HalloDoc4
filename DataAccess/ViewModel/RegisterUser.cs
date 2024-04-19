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
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one digit")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters long")]

        [Required(ErrorMessage = "Please enter Your Password.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Please Confirm Your Password.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string? ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public int? roleid { get; set; }
        public string AspNetUserId { get; set; }
        public int AdminId { get; set; }
        public int?  SubRoleId{get;set;}
        public int PhysicianId { get; set; }
        public int UserId { get; set; }

    }
   

}
