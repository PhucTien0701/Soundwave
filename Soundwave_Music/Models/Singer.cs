using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Soundwave_Music.Models
{
    [Table("Singer")]
    public partial class Singer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Singer()
        {
            Albums = new HashSet<Album>();
            Songs = new HashSet<Song>();
            Videos = new HashSet<Video>();
        }

        //Singer ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Singer ID")]
        public int Singer_id { get; set; }

        //Singer Name
        [Required(ErrorMessage = "You have to input singer name.")]
        [StringLength(100)]
        [Display(Name = "Singer Name")]
        public string Singer_name { get; set; }

        //Singer Image
        [Required(ErrorMessage = "You have to upload an image for singer.")]
        [StringLength(500)]
        public string Image { get; set; }

        //Create Date
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date")]
        public DateTime Create_date { get; set; }

        //Create By
        [StringLength(100)]
        [Display(Name = "Create By")]
        public string Create_by { get; set; }

        //Singer Status
        [StringLength(1)]
        public string Singer_status { get; set; }

        //Choose singer area
        [Required]
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
