namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Video_Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Video_Comment()
        {
            Like_Video_Comment = new HashSet<Like_Video_Comment>();
        }

        [Key]
        public int Video_comment_id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Create_date { get; set; }

        public int Video_id { get; set; }

        public int User_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Video_Comment> Like_Video_Comment { get; set; }

        public virtual User User { get; set; }

        public virtual Video Video { get; set; }
    }
}
