namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Like_Album_Comment
    {
        [Key]
        public int Like_Album_comment_id { get; set; }

        [StringLength(1)]
        public string React_like { get; set; }

        public DateTime Create_date { get; set; }

        public int Album_comment_id { get; set; }

        public int User_id { get; set; }

        public virtual Album_Comment Album_Comment { get; set; }

        public virtual User User { get; set; }
    }
}
