namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdminInfo")]
    public partial class AdminInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminID { get; set; }

        [Required]
        [StringLength(16)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string PassWord { get; set; }

        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        [Required]
        [StringLength(32)]
        public string Email { get; set; }

        public byte Sex { get; set; }

        public byte DelFlag { get; set; }
    }
}
