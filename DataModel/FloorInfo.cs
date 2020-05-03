namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FloorInfo")]
    public partial class FloorInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string FloorID { get; set; }

        [Required]
        [StringLength(16)]
        public string FloorName { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }
    }
}
