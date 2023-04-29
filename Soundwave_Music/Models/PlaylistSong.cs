using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Models
{
    [Table("PlaylistSong")]
    public class PlaylistSong
    {
        [Key]
        [Column(Order = 0)]
        public int Playlist_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public int Song_id { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual Song Song { get; set; }
    }

}