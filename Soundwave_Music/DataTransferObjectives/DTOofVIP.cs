using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofVIP
    {
        //=>Product Name
        [Required(ErrorMessage = "Please input VIP Package Name")]
        public string product_name { get; set; }
        //=>
        public int product_id { get; set; }
        //=>
        public string slug { get; set; }
        //=>
        public string description { get; set; }
        //=>
        [Required(ErrorMessage = "Please input price")]
        public double price { get; set; }
        //=>
        public string price_format { get; set; }
        //=>
        [Required(ErrorMessage = " ")]
        public string quantity { get; set; }
        //=>
        public int product_img_id { get; set; }
        [Required(ErrorMessage = "Please choose image")]
        public string Image { get; set; }
        //=>
        public string create_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_date { get; set; }
        //=>
        public string update_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_date { get; set; }
        //=>
        public string status { get; set; }
        //=>
        public long buyturn { get; set; }
        //=>
        public int count_Order_detail { get; set; }

    }
}