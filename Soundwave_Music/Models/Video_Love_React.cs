namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Video_Love_React
    {
        [Key]
        public int Video_love_react_id { get; set; }

        [StringLength(1)]
        public string React_love { get; set; }

        public DateTime Create_date { get; set; }

        public int Video_id { get; set; }

        public int User_id { get; set; }

        public virtual User User { get; set; }

        public virtual Video Video { get; set; }
    }
}
