using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofPayment
    {
        public long Order_id { get; set; }
        public double Amount { get; set; }
        public DateTime Create_date { get; set; }
        public string Status { get; set; }
    }
}