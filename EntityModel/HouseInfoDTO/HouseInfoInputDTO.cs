using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.HouseInfoDTO
{
    /// <summary>
    /// HouseInfo输入数据
    /// </summary>
  public  class HouseInfoInputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string HousID { get; set; }

        [Required]
        [StringLength(16)]
        public string VillageID { get; set; }

        [Required]
        [StringLength(16)]
        public string FloorID { get; set; }

        [Required]
        [StringLength(16)]
        public string HouseType { get; set; }

        //public bool State { get; set; }
        //[StringLength(20)]
        //public string UserID { get; set; }

        [Required]
        [StringLength(16)]
        public string HouseArea { get; set; }

        [StringLength(16)]
        public string Purpose { get; set; }

        [Required]
        [StringLength(30)]
        public string PropertyCost { get; set; }
    }
}
