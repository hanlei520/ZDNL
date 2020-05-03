namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string UserID { get; set; }

        [Required]
        [StringLength(32)]
        public string PassWord { get; set; }

        [Required]
        [StringLength(16)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string IDNumber { get; set; }

        public byte Sex { get; set; }

        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        [StringLength(16)]
        public string HousID { get; set; }
    }
}
