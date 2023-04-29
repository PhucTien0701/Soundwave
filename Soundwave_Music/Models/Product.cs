namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Detail = new HashSet<Order_Detail>();
        }

        [Key]
        public int product_id { get; set; }

        [Required]
        [StringLength(200)]
        public string product_name { get; set; }

        [StringLength(200)]
        public string slug { get; set; }

        public double price { get; set; }

        public long buyturn { get; set; }

        [Required]
        [StringLength(10)]
        public string quantity { get; set; }

        [StringLength(1)]
        public string status { get; set; }

        [Required]
        [StringLength(100)]
        public string create_by { get; set; }

        public DateTime create_date { get; set; }

        [StringLength(100)]
        public string update_by { get; set; }

        public DateTime update_date { get; set; }

        [StringLength(500)]
        public string image { get; set; }

        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Detail { get; set; }
    }
}
