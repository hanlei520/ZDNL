namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RepairInfo")]
    public partial class RepairInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string RepairID { get; set; }

        [Required]
        [StringLength(16)]
        public string UserID { get; set; }

        [Required]
        [StringLength(16)]
        public string HousID { get; set; }

        [Required]
        [StringLength(50)]
        public string Rreason { get; set; }

        public DateTime RsubmitData { get; set; }

        public DateTime? RsolveData { get; set; }

        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        public int? State { get; set; }

        [StringLength(16)]
        public string AdminID { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
