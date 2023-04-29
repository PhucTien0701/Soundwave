namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Song_Love_React
    {
        [Key]
        public int Song_love_react_id { get; set; }

        public DateTime Create_date { get; set; }

        public int Song_id { get; set; }

        public int User_id { get; set; }

        [StringLength(1)]
        public string React_love { get; set; }

        public virtual Song Song { get; set; }

        public virtual User User { get; set; }
    }
}
