using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class ComplaInfoOutput
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

        [StringLength(50)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(16)]
        public string UserName { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminName { get; set; }
    }
}
