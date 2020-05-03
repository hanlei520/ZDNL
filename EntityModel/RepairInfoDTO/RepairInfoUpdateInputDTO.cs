using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.RepairInfoDTO
{
  public  class RepairInfoUpdateInputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string RepairID { get; set; }

        public DateTime? RsolveData { get; set; }
        public int? State { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
