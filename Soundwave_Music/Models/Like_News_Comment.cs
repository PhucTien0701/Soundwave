namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Like_News_Comment
    {
        [Key]
        public int Like_news_comment_id { get; set; }

        [StringLength(1)]
        public string React_like { get; set; }

        public DateTime Create_date { get; set; }

        public int News_comment_id { get; set; }

        public int User_id { get; set; }

        public virtual News_Comment News_Comment { get; set; }

        public virtual User User { get; set; }
    }
}
