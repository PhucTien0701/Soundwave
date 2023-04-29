using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofVideo
    {
        public int Video_id { get; set; }

        public string Video_name { get; set; }

        public string Image { get; set; }

        public int View_count { get; set; }

        public DateTime Release_date { get; set; }

        public string Lyric { get; set; }

        public string Create_by { get; set; }

        public string Video_File_Upload { get; set; }

        public string Video_status { get; set; }

        public int Singer_id { get; set; }

        public int Composer_id { get; set; }

        public int Genre_id { get; set; }

        public int Supplier_id { get; set; }

        public int Area_id { get; set; }

        public int Videos_comment { get; set; }
        
        public int Videos_love_react { get; set; }

        public string Singer { get; set; }

        public string Composer { get;set; }

        public string Genre { get;set; }

        public string Supplier { get; set; }

        public string Area { get; set; }
    }
}