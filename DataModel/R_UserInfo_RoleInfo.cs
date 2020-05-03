namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class R_UserInfo_RoleInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminID { get; set; }

        [Required]
        [StringLength(16)]
        public string RoleID { get; set; }
    }
}
