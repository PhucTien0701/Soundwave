using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Soundwave_Music.Models
{
    [Table("Album")]
    public partial class Album
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Album()
        {
            Album_Comment = new HashSet<Album_Comment>();
            Album_Love_React = new HashSet<Album_Love_React>();
            Songs = new HashSet<Song>();
        }

        //Album ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Album ID")]
        public int Album_id { get; set; }

        //Album Image
        [Required(ErrorMessage = "You have to upload an image for Album")]
        [StringLength(500)]
        public string Image { get; set; }

        //Album Name
        [Required(ErrorMessage = "You have to input album name.")]
        [StringLength(100)]
        public string Album_name { get; set; }

        //Create Date
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date")]
        public DateTime Create_date { get; set; }

        //Create By
        [StringLength(100)]
        [Display(Name = "Create By")]
        public string Create_by { get; set; }

        //Album Status
        [StringLength(1)]
        public string Album_status { get; set; }

        public int Genre_id { get; set; }

        public int Singer_id { get; set; }

        public int Area_id { get; set; }

        public int view_count { get; set; }

        public virtual Area Area { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Singer Singer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album_Comment> Album_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album_Love_React> Album_Love_React { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song> Songs { get; set; }
    }
}
