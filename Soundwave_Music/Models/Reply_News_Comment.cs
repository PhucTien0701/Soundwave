namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reply_News_Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reply_News_Comment()
        {
            Like_Reply_News_Comment = new HashSet<Like_Reply_News_Comment>();
        }

        [Key]
        public int Reply_news_comment_id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Create_date { get; set; }

        public int News_comment_id { get; set; }

        public int User_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like_Reply_News_Comment> Like_Reply_News_Comment { get; set; }

        public virtual News_Comment News_Comment { get; set; }

        public virtual User User { get; set; }
    }
}
