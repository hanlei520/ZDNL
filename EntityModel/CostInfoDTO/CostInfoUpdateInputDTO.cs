using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.CostInfoDTO
{
  public  class CostInfoUpdateInputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string CostID { get; set; }
        [Required]
        [StringLength(16)]
        public string CostName { get; set; }

        public DateTime RsubmitDate { get; set; }

        public DateTime RsolveDate { get; set; }

        public decimal WaterMeter { get; set; }

        public decimal ElectricMete { get; set; }

        public decimal WaterPrice { get; set; }

        public decimal WirePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal SumMoney { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminID { get; set; }

        public bool State { get; set; }
    }
}
