using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Models
{
    [Table("Role_Permission")]
    public class Role_Permission
    {
        //Role ID
        [Key]
        [Column(Order = 0)]
        public int Role_id { get; set; }

        //Permission ID
        [Key]
        [Column(Order = 1)]
        public int Permission_id { get; set; }

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}