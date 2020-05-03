namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VillageInfo")]
    public partial class VillageInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string VillageID { get; set; }

        [Required]
        [StringLength(16)]
        public string VillageName { get; set; }
    }
}
