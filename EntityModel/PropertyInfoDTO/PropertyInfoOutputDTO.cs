using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.PropertyInfoDTO
{
  public  class PropertyInfoOutputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string PropertyID { get; set; }

        [Required]
        [StringLength(16)]
        public string HouseID { get; set; }

        public decimal PropertyCost { get; set; }

        public string Month { get; set; }

        public int State { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminID { get; set; }
        [Required]
        [StringLength(16)]
        public string UserName { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminName { get; set; }
    }
}
