namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Album_Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Album_Comment()
        {
            Like_Album_Comment = new HashSet<Like_Album_Comment>();
        }

        [Key]
        public int Album_comment_id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Create_date { get; set; }

        public int Album_id { get; set; }

        public int User_id { get; set; }

        public virtual Album Album { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Album_Comment> Like_Album_Comment { get; set; }
    }
}
