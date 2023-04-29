using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofPlaylist
    {
        public List<PlaylistSongsCheckbox> Songs { get; set; }
        public int Playlist_id { get; set; }

        public string Playlist_name { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Create_date { get; set; }

        [StringLength(100)]
        public string Create_by { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Update_date { get; set; }

        [StringLength(100)]
        public string Update_by { get; set; }

        public int User_id { get; set; }

        public int Song_id { get; set; }

        [StringLength(100)]
        public string Song_name { get; set; }

        public class PlaylistSongsCheckbox
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool Checked { get; set; }
        }
    }
}