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
    [Table("DrugUnits")]
    public class DrugUnit
    {
        [Key]
        [Display(Name = "药物单位ID")]
        public virtual int DrugUnitID { get; set; }
        [Display(Name = "原HIS药物单位名称")]
        public virtual string Origin_UNIT { get; set; }
        [Display(Name = "是否整袋（瓶）使用")]
        public virtual bool IsUseByBottle { get; set; }
    }
}
