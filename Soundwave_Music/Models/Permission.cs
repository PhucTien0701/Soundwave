namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Permission")]
    public partial class Permission
    {
        //Permission ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Permission_id { get; set; }

        //Permission Name
        [Required(ErrorMessage = "Please input permission name.")]
        [StringLength(100)]
        public string Permission_name { get; set; }

        public virtual ICollection<Role_Permission> Role_Permission { get; set; }
    }
}
