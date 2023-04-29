namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Playlist")]
    public partial class Playlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Playlist_id { get; set; }

        [Required]
        [StringLength(100)]
        public string Playlist_name { get; set; }

        //Image
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Create_date { get; set; }

        [StringLength(100)]
        public string Create_by { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Update_date { get; set; }

        [StringLength(100)]
        public string Update_by { get; set; }

        public int User_id { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<PlaylistSong> PlaylistSong { get; set; }
    }
}
