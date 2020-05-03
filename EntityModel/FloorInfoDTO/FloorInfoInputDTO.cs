using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.FloorInfoDTO
{
 public   class FloorInfoInputDTO
    {
        public int ID { get; set; }
        [Required]
        [StringLength(16)]
        public string FloorID { get; set; }

        [Required]
        [StringLength(16)]
        public string FloorName { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }
    }
}
