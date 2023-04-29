using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Common
{
    public class Pagination
    {
        public string Display { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }
        public int Page { get; set; }
    }
}