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
    [Table("AntibioticLevels")]
    public class AntibioticLevel
    {
        [Key]
        [Display(Name = "抗菌药物等级ID")]
        public virtual int AntibioicLevelID { get; set; }
        [Display(Name = "原HIS抗菌药物等级")]
        public virtual int? Origin_KSSDJID { get; set; }
        [Display(Name = "是否为抗菌药物")]
        public virtual bool IsAntibiotic { get; set; }
        [Display(Name = "是否为非限制性抗菌药物")]
        public virtual bool IsNonRestrict { get; set; }
        [Display(Name = "是否为限制性抗菌药物")]
        public virtual bool IsRestrict { get; set; }

        [Display(Name = "是否为特殊性抗菌药物")]
        public virtual bool IsSpecial { get; set; }
        [Display(Name = "抗菌药物等级名称")]
        public virtual string AntibioticLevelName { get; set; }
        [Display(Name = "抗菌药物等级备注")]
        public virtual string AntibioticLevelRemarks { get; set; }
    }
}
