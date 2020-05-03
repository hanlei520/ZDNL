namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleInfo")]
    public partial class RoleInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string RoleID { get; set; }

        [Required]
        [StringLength(16)]
        public string RoleName { get; set; }

        [Required]
        [StringLength(32)]
        public string Description { get; set; }

        public DateTime AddTime { get; set; }

        public byte DelFlag { get; set; }

        public DateTime? DelTime { get; set; }
    }
}
