namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supplier")]
    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            Songs = new HashSet<Song>();
            Videos = new HashSet<Video>();
        }

        [Key]
        public int Supplier_id { get; set; }

        [Required]
        [StringLength(100)]
        public string Supplier_name { get; set; }

        [StringLength(500)]
        public string Image { get; set; }

        [StringLength(1)]
        public string Supplier_status { get; set; }

        public DateTime Create_date { get; set; }

        [Required]
        [StringLength(100)]
        public string Create_by { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song> Songs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video> Videos { get; set; }
    }
}
