using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofAlbum
    {
        public int Album_id { get; set; }
        public string Image { get; set; }
        public string Album_name { get; set; }
        public DateTime Create_date { get; set; }
        public string Create_by { get; set; }
        public string Album_status { get; set; }
        public int Genre_id { get; set; }
        public int Singer_id { get; set; }
        public int Area_id { get; set; }
        public int Albums_comment { get; set; }
        public int Albums_love_react { get; set; }
        public int Songs { get; set; }
        public string Singer { get; set; }
        public int View_count { get; set; }
    }
}