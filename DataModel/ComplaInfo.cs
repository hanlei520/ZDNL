namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ComplaInfo")]
    public partial class ComplaInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string ComplaID { get; set; }

        [Required]
        [StringLength(16)]
        public string CompType { get; set; }

        [Required]
        [StringLength(16)]
        public string HousID { get; set; }

        [Required]
        [StringLength(16)]
        public string UserID { get; set; }

        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Contents { get; set; }

        public DateTime RsubmitData { get; set; }

        public DateTime? RsolveData { get; set; }

        public int State { get; set; }

        [StringLength(16)]
        public string AdminID { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
