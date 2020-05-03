using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.VillageInfoDTO
{
  public  class VillageInfoInputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string VillageID { get; set; }

        [Required]
        [StringLength(16)]
        public string VillageName { get; set; }
    }
}
