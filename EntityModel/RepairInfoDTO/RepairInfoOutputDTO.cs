using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.RepairInfoDTO
{
   public class RepairInfoOutputDTO
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
