namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Album_Love_React
    {
        [Key]
        public int Album_love_react_id { get; set; }

        [StringLength(1)]
        public string React_love { get; set; }

        public DateTime Create_date { get; set; }

        public int Album_id { get; set; }

        public int User_id { get; set; }

        public virtual Album Album { get; set; }

        public virtual User User { get; set; }
    }
}
