namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Song_Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Song_Comment()
        {
            Like_Song_Comment = new HashSet<Like_Song_Comment>();
        }

        [Key]
        public int Song_comment_id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Create_date { get; set; }

        public int Song_id { get; set; }

        public int User_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Song_Comment> Like_Song_Comment { get; set; }

        public virtual Song Song { get; set; }

        public virtual User User { get; set; }
    }
}
