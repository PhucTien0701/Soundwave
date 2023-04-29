using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofSong
    {
        //Song items
        public int Song_id { get; set; }
        public string Image { get; set; }
        public string Song_name { get; set; }
        public int View_count { get; set; }
        public string Music_File_Upload { get; set; }
        public DateTime Release_date { get; set; }
        public string Lyric { get; set; }
        public string Song_status { get; set; }
        public string Create_by { get; set; }
        public string Singer { get; set; }
        public string Composer { get;set; }
        //Choose album
        [Required(ErrorMessage = "Please choose album")]
        public int Album_id { get; set; }
        //choose singer
        [Required(ErrorMessage = "Please choose singer")]
        public int Singer_id { get; set; }
        //choose composer
        [Required(ErrorMessage = "Please choose composer")]
        public int Composer_id { get; set; }
        //choose genre
        [Required(ErrorMessage = "Please choose genre")]
        public int Genre_id { get; set; }
        //choose supplier
        [Required(ErrorMessage = "Please choose supplier")]
        public int Supplier_id { get; set; }
        //area
        public int Area_name { get; set; }
        //Comment song
        public int songs_comment { get; set; }
        //Love song
        public int songs_love_react { get; set; }

    }
}