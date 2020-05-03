using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.ComplaInfoDTO
{
 public   class ComplaInfoUpdateInputDTO
    {
        [Required]
        [StringLength(16)]
        public string ComplaID { get; set; }
        public DateTime RsubmitData { get; set; }

        public DateTime? RsolveData { get; set; }

        public int State { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
