using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2Domain.Models
{
    public partial class Usages
    {
        [Key]
        public Guid UsageID { get; set; }
        [Required]
        public string UsageName { get; set; }
        public string Remarks { get; set; }
        public bool IsInjection { get; set; }
    }
}
