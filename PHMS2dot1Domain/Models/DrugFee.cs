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
    [Table("DrugFees")]
    public class DrugFee
    {
        [Key]
        [Display(Name = "药物费用ID")]
        public virtual Guid DrugFeeID { get; set; }
        [Display(Name = "原HIS费用表ID")]
        public virtual Guid? Origin_ID { get; set; }
        [Display(Name = "药物记录ID")]
        public virtual Guid DrugRecordID { get; set; }
        [Display(Name = "药物单价")]
        public virtual Decimal UnitPrice { get; set; }
        [Display(Name = "原HIS药物单位")]
        public virtual string Origin_Unit { get; set; }
        [Display(Name = "药物数量")]
        public virtual Decimal Quantity { get; set; }
        [Display(Name = "实际总价")]
        public virtual Decimal ActualPrice { get; set; }
        [Display(Name = "收费时间")]
        public virtual DateTime ChargeTime { get; set; }
        public virtual InPatientDrugRecord DrugRecord { get; set; }
    }
}
