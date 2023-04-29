using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Models.UserManagementModels
{
    public class Sign_Up
    {
        //User ID
        public int User_id { get; set; }

        //Password
        [Required(ErrorMessage = "Please input your password")]
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Your password must have 8 characters including at least 1 lowercase letter, 1 uppercase letter, 1 number, and 1 special letter.")]
        public string Password { get; set; }

        //Confirm password
        [Required(ErrorMessage = "Please input your password again", AllowEmptyStrings = false)]
        [Compare("Password", ErrorMessage = "Your confirm password is not match with password. Please input again.")]
        public string Confirm_password { get; set; }

        //User Email
        [Required(ErrorMessage = "Please input your email address")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Please input the right format of email. Exp: abc@gmail.com")]
        public string Email { get; set; }

        //Avatar
        [StringLength(500)]
        public string Avatar { get; set; }

        //User Full Name
        [Required(ErrorMessage = "Please input your full name")]
        [StringLength(50)]
        public string Full_name { get; set; }

        //Phone number
        [Required(ErrorMessage = "Please input your phone number")]
        [StringLength(10)]
        [MinLength(10, ErrorMessage = "Your phone number is not enough")]
        [RegularExpression("^(0|84)([0-9]{9})$", ErrorMessage = "Phone number must begin with 0 and just only input number in this field.")]
        public string Phone_number { get; set; }

        //Gender
        [Required(ErrorMessage = "Please choose your gender")]
        [StringLength(1)]
        public string Gender { get; set; }

        //Date of birth
        [Required(ErrorMessage = "Please input your date of birth")]
        //[Range(typeof(DateTime), "01-01-1922", "12-30-2009", ErrorMessage = "Your age is not enough to sign up.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date_of_birth { get; set; }

        //Create date
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime Create_date { get; set; }

        //Update date
        [DisplayFormat(DataFormatString = "0:dd-MM-yyyy HH:mm", ApplyFormatInEditMode = true)]
        public DateTime Update_date { get; set; }

        //Verification code
        [StringLength(100)]
        public string Verification_code { get; set; }

        //User Status
        [StringLength(1)]
        public string User_Status { get; set; }

        //Choose Role
        [Required(ErrorMessage = "Choose role")]
        [StringLength(1)]
        public string Role { get; set; }
    }
}