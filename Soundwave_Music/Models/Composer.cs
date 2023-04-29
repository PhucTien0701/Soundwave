using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Soundwave_Music.Models
{
    [Table("Composer")]
    public partial class Composer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composer()
        {
            Songs = new HashSet<Song>();
            Videos = new HashSet<Video>();
        }

        //Compser ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Composer ID")]
        public int Composer_id { get; set; }

        //Composer Name
        [Required(ErrorMessage = "You have to input composer name.")]
        [StringLength(100)]
        [Display(Name = "Composer Name")]
        public string Composer_name { get; set; }

        //Composer Image
        [Required(ErrorMessage = "You have to upload an image for composer.")]
        [StringLength(500)]
        public string Image { get; set; }

        //Create date
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date")]
        public DateTime Create_date { get; set; }

        //Create By
        [StringLength(100)]
        [Display(Name = "Create By")]
        public string Create_by { get; set; }

        //Composer Status
        [StringLength(1)]
        public string Composer_status { get; set; }

        public int Area_id { get; set; }

        public virtual Area Area { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song> Songs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video> Videos { get; set; }
    }
}
