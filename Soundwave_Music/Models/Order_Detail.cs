namespace Soundwave_Music.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int product_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Order_id { get; set; }

        public double Price { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        public int quantity { get; set; }

        public DateTime Create_date { get; set; }

        [Required]
        [StringLength(100)]
        public string Create_by { get; set; }

        public DateTime Update_date { get; set; }

        [Required]
        [StringLength(100)]
        public string Update_by { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
