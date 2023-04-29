namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Video")]
    public partial class Video
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Video()
        {
            Video_Comment = new HashSet<Video_Comment>();
            Video_Love_React = new HashSet<Video_Love_React>();
        }

        [Key]
        public int Video_id { get; set; }

        [Required]
        [StringLength(100)]
        public string Video_name { get; set; }

        [Required]
        [StringLength(500)]
        public string Image { get; set; }

        public int View_count { get; set; }

        public DateTime Release_date { get; set; }

        public string Lyric { get; set; }

        [StringLength(100)]
        public string Create_by { get; set; }

        [StringLength(500)]
        public string Video_File_Upload { get; set; }

        [StringLength(1)]
        public string Video_status { get; set; }

        public int Singer_id { get; set; }

        public int Composer_id { get; set; }

        public int Genre_id { get; set; }

        public int Supplier_id { get; set; }

        public int Area_id { get; set; }

        public virtual Area Area { get; set; }

        public virtual Composer Composer { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Singer Singer { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video_Comment> Video_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video_Love_React> Video_Love_React { get; set; }
    }
}
