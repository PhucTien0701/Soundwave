using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Models.UserManagementModels
{
    public class Change_Password
    {
        //Old Password
        [Required(ErrorMessage = "Please input your old password", AllowEmptyStrings = false)]
        public string Input_old_password { get; set; }

        //New Password
        [Required(ErrorMessage = "Please input your new password", AllowEmptyStrings = false)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Your password must have 8 characters including at least 1 lowercase letter, 1 uppercase letter, 1 number, and 1 special letter.")]
        public string Input_new_password { get; set; }

        //Confirm password
        [Required(ErrorMessage = "Please input your password again", AllowEmptyStrings = false)]
        [Compare("Input_new_password", ErrorMessage = "Your confirm password is not match with new password. Please input again.")]
        public string Confirm_password { get; set; }

        //Avatar
        public string Avatar { get; set; }

        //Full name
        public string Full_name { get; set; }
    }
}