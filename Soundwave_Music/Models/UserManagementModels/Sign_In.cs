using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Models.UserManagementModels
{
    public class ExternalLoginConfirm
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLogin
    {
        public string ReturnUrl { get; set; }
    }

    public class Sign_In
    {
        //User id
        public int User_id { get; set; }

        //Email
        [Required(ErrorMessage = "Please input your email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //Password
        [Required(ErrorMessage = "Please input your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Role
        [StringLength(1)]
        public string Role { get; set; }

        //User Name
        public string Full_name { get; set; }

        //Avatar
        public string Avatar { get; set; }
    }
}