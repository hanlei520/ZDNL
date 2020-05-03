using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.RoleInfoDTO
{
    public class RoleInfoInputDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string RoleID { get; set; }

        [Required]
        [StringLength(16)]
        public string RoleName { get; set; }

        [Required]
        [StringLength(32)]
        public string Description { get; set; }
    }
}
