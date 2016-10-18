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
    [Table("OutPatientDrugRecords")]
    public class OutPatientDrugRecord
    {
        [Display(Name = "门诊药物记录ID")]
        public virtual Guid OutPatientDrugRecordID { get; set; }

        [Display(Name = "原HIS门诊处方明细ID")]
        public virtual Guid? Origin_CFMXID { get; set; }

        [Display(Name = "门诊处方ID")]
        public virtual Guid OutPatientPrescriptionID { get; set; }

        [Display(Name = "原HIS抗菌药物等级")]
        public virtual int? Origin_KSSDJ { get; set; }

        [Display(Name = "原HIS药物CJID")]
        public virtual int? Origin_CJID { get; set; }

        [Display(Name = "药物品名")]
        public virtual string ProductName { get; set; }

        [Display(Name = "是否基本药物")]
        public virtual bool IsEssential { get; set; }

        [Display(Name = "是否西药")]
        public virtual bool IsWesternMedicine { get; set; }

        [Display(Name = "是否中成药")]
        public virtual bool IsChinesePatentMedicine { get; set; }

        [Display(Name = "药物剂型")]
        public virtual string DosageForm { get; set; }

        [Display(Name = "Ddd值")]
        public virtual Decimal Ddd { get; set; }

        [Display(Name = "原HIS药物用法")]
        public virtual string Origin_YFMC { get; set; }

        [Display(Name = "药物单价")]
        public virtual Decimal UnitPrice { get; set; }

        [Display(Name = "药物单位")]
        public virtual string UnitName { get; set; }

        [Display(Name = "药物数量")]
        public virtual Decimal Quantity { get; set; }

        [Display(Name = "实际总价")]
        public virtual Decimal ActualPrice { get; set; }

        public virtual OutPatientPrescription OutPatientPrescription { get; set; }
    }
}
