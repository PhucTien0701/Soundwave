using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Soundwave_Music.Models
{
    [Table("Area")]
    public partial class Area
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Area()
        {
            Albums = new HashSet<Album>();
            Composers = new HashSet<Composer>();
            Genres = new HashSet<Genre>();
            Singers = new HashSet<Singer>();
            Songs = new HashSet<Song>();
            Videos = new HashSet<Video>();
        }

        //Area ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Area_id { get; set; }

        //Area Name
        [Required]
        [Display(Name = "Area Name")]
        [StringLength(100)]
        public string Area_name { get; set; }

        //Image
        [Required(ErrorMessage = "You have to upload an image for area.")]
        [StringLength(500)]
        public string Image { get; set; }

        //Creat date
        [Display(Name = "Create Date")]
        public DateTime Create_date { get; set; }

        //Create by
        [Required]
        [Display(Name = "Create By")]
        [StringLength(100)]
        public string Create_by { get; set; }

        //Area status
        [StringLength(1)]
        public string Area_status { get; set; }

        //Area update date
        [Display(Name = "Update Date")]
        public DateTime Update_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album> Albums { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> Composers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Genre> Genres { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Singer> Singers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song> Songs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video> Videos { get; set; }
    }
}
