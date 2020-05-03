using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.AdminInfoDTO
{
  public  class AdminInfoInputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string AdminID { get; set; }

        [Required]
        [StringLength(16)]
        public string Name { get; set; }


        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        [Required]
        [StringLength(32)]
        public string Email { get; set; }

        public byte Sex { get; set; }

    }
}
