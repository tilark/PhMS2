using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhMS2dot1Domain.Models
{ 
    [Table("DrugUsages")]
    public class DrugUsage
    {
        [Key]
        [Display(Name = "药物用法ID")]
        public virtual int DrugUsageID { get; set; }
        [Display(Name = "药物用法名称（原HIS）")]
        public virtual string Origin_ORDER_USAGE { get; set; }
        [Display(Name = "是否用于注射")]
        public virtual bool IsUseForInjection { get; set; }
        [Display(Name = "是否用于静脉注射")]
        public virtual bool IsUseForIntravenousTransfusion { get; set; }
        [Display(Name = "药物用法备注")]
        public virtual string DrugUsageRemarks { get; set; }
    }
}
