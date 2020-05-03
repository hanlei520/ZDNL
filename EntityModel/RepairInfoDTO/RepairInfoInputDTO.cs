using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.RepairInfoDTO
{
   public class RepairInfoInputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string HousID { get; set; }

        [Required]
        [StringLength(50)]
        public string Rreason { get; set; }

        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        [StringLength(16)]
        public string AdminID { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

    }
}
