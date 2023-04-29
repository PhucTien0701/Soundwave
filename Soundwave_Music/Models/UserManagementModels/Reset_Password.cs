using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Models.UserManagementModels
{
    public class Reset_Password
    {
        //Input new password
        [Required(ErrorMessage = "Please input new password", AllowEmptyStrings = false)]
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Your password must have 8 characters including at least 1 lowercase letter, 1 uppercase letter, 1 number, and 1 special letter.")]
        [DataType(DataType.Password)]
        public string Input_new_password { get; set; }

        //Input confirm password
        [Required(ErrorMessage = "Please input your new password again", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("Input_new_password", ErrorMessage = "The password confirm is not match with new password. Please input again.")]
        public string Confirm_new_password { get; set; }

        //Reset password
        [Required]
        public string Reset_password { get; set; }
    }
}