namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PropertyInfo")]
    public partial class PropertyInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string PropertyID { get; set; }

        [Required]
        [StringLength(16)]
        public string HousID { get; set; }

        public decimal PropertyCost { get; set; }

        [Required]
        [StringLength(16)]
        public string Month { get; set; }

        public int State { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminID { get; set; }
    }
}
