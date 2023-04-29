using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Soundwave_Music.Models
{
    [Table("Genre")]
    public partial class Genre
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Genre()
        {
            Albums = new HashSet<Album>();
            Songs = new HashSet<Song>();
            Videos = new HashSet<Video>();
        }

        //Genre id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Genre ID")]
        public int Genre_id { get; set; }

        //Genre name
        [Required(ErrorMessage = "Please input genre name")]
        [StringLength(100)]
        [Display(Name = "Genre Name")]
        public string Genre_name { get; set; }

        //Image
        [Required(ErrorMessage = "You have to upload an image for genre.")]
        [StringLength(500)]
        public string Image { get; set; }

        //Create date
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Create_date { get; set; }

        //Create by
        [StringLength(100)]
        public string Create_by { get; set; }

        //Genre status
        [StringLength(1)]
        public string Genre_status { get; set; }

        //Area name
        [Required]
        [Display(Name = "Area")]
        public int Area_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album> Albums { get; set; }

        public virtual Area Area { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song> Songs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video> Videos { get; set; }
    }
}
