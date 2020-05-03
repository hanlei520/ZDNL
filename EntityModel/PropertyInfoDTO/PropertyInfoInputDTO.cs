using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.PropertyInfoDTO
{
  public  class PropertyInfoInputDTO
    {
        public int ID { get; set; }


        [Required]
        [StringLength(16)]
        public string HousID { get; set; }

        public decimal PropertyCost { get; set; }

        [Required]
        [StringLength(16)]
        public string Month { get; set; }

        public int State { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminID { get; set; }
    }
}
