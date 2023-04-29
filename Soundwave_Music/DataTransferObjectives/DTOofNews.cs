using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofNews
    {
        public int News_id { get; set; }

        [Required(ErrorMessage = "News title is required")]
        [StringLength(500)]
        public string News_title { get; set; }

        [Required(ErrorMessage = "News content is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please choose an image for this news")]
        [StringLength(500)]
        public string Image { get; set; }

        public int View_count { get; set; }

        public DateTime Create_date { get; set; }

        public DateTime Update_date { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        public int User_id { get; set; }
    }
}