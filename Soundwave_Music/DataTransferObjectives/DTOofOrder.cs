using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofOrder
    {
        //=>
        public int order_id { get; set; }
        //=>
        public string Name { get; set; }
        //=>
        public string Email { get; set; }
        //=>
        public string Phone { get; set; }
        //=>
        public string create_by { get; set; }
        //=>
        public int total_quantity { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //=>
        public string update_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime order_date { get; set; }
        //=>
        public string status { get; set; }
        //=>
        public int payment_id { get; set; }
        //=>
        public string payment_name { get; set; }
        //=>
        public string product_name { get; set; }
        //=>
        public string payment_transaction { get; set; }
        //=>
        [Required(ErrorMessage = "Input name")]
        public string order_username { get; set; }
        //=>
        [Required(ErrorMessage = "Input number phone")]
        public string order_phonenum { get; set; }
        //=>
        public double price { get; set; }
        //=>
        public double temporary { get; set; }
        //=>
        public double total_price { get; set; }
        //=>
        public int user_id { get; set; }
    }
}