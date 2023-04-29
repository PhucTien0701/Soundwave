using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web;

namespace Soundwave_Music.Models
{
    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Album_Comment = new HashSet<Album_Comment>();
            Album_Love_React = new HashSet<Album_Love_React>();
            Like_Album_Comment = new HashSet<Like_Album_Comment>();
            Like_News_Comment = new HashSet<Like_News_Comment>();
            Like_Reply_News_Comment = new HashSet<Like_Reply_News_Comment>();
            Like_Song_Comment = new HashSet<Like_Song_Comment>();
            Like_Video_Comment = new HashSet<Like_Video_Comment>();
            News = new HashSet<News>();
            News_Comment = new HashSet<News_Comment>();
            Orders = new HashSet<Order>();
            Playlists = new HashSet<Playlist>();
            Reply_News_Comment = new HashSet<Reply_News_Comment>();
            Song_Comment = new HashSet<Song_Comment>();
            Song_Love_React = new HashSet<Song_Love_React>();
            Video_Comment = new HashSet<Video_Comment>();
            Video_Love_React = new HashSet<Video_Love_React>();
        }

        [Key]
        //User ID
        public int User_id { get; set; }

        //Password
        [Display(Name = "Password")]
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Your password must have 8 characters including at least 1 lowercase letter, 1 uppercase letter, 1 number, and 1 special letter.")]
        public string Password { get; set; }

        //Email
        [StringLength(100)]
        public string Email { get; set; }

        //Avatar
        [StringLength(500)]
        public string Avatar { get; set; }

        //User Full Name
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please input your full name")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Full_name { get; set; }

        //Phone Number
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Please input your phone number")]
        [StringLength(10)]
        [RegularExpression("^(0)([0-9]{9})$", ErrorMessage = "This field begin with 0 ,and just allow input number")]
        public string Phone_number { get; set; }

        //Genre
        [Required(ErrorMessage = "Please choose your gender")]
        [StringLength(1)]
        public string Gender { get; set; }

        //Date of birth
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please input your date of birth")]
        //[Range(typeof(DateTime), "01-01-1922", "12-30-2009", ErrorMessage = "Your age is not enough to sign up.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date_of_birth { get; set; }

        //Create date
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Create_date { get; set; }

        //Update date
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Update_date { get; set; }

        //Verification code
        [StringLength(100)]
        public string Verification_code { get; set; }

        //User Status
        [StringLength(1)]
        public string User_Status { get; set; }

        //Expired time
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Expired { get; set; }

        //Role Id refereces Role table
        [Display(Name = "Role ID")]
        public int? Role_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album_Comment> Album_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album_Love_React> Album_Love_React { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Album_Comment> Like_Album_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_News_Comment> Like_News_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Reply_News_Comment> Like_Reply_News_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Song_Comment> Like_Song_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Video_Comment> Like_Video_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News_Comment> News_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Playlist> Playlists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reply_News_Comment> Reply_News_Comment { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song_Comment> Song_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song_Love_React> Song_Love_React { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video_Comment> Video_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video_Love_React> Video_Love_React { get; set; }

        [Display(Name = "Avatar Upload")]
        [NotMapped]
        public HttpPostedFileBase AvatarUpload { get; set; }
    }
}
