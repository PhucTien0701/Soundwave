namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Detail = new HashSet<Order_Detail>();
        }

        [Key]
        public int Order_id { get; set; }

        public DateTime Order_date { get; set; }

        [Required]
        [StringLength(100)]
        public string Order_create_by { get; set; }

        public DateTime Create_date { get; set; }

        public DateTime Update_date { get; set; }

        [Required]
        [StringLength(100)]
        public string Update_by { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        [StringLength(1)]
        public string Payment_transaction { get; set; }

        public double Total { get; set; }

        public int User_id { get; set; }

        public int Payment_id { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Detail { get; set; }
    }
}
