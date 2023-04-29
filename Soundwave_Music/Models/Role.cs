namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            Users = new HashSet<User>();            
        }

        //Role ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Role_id { get; set; }

        //Role Name
        [Required(ErrorMessage = "Please input role name")]
        [StringLength(100)]
        public string Role_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Role_Permission> Role_Permission { get; set; }
    }
}
