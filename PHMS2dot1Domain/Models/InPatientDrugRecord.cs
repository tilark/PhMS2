using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ClassViewModelToDomain;

namespace PhMS2dot1Domain.Models
{
    [Table("InPatientDrugRecords")]
    public class InPatientDrugRecord
    {
        public InPatientDrugRecord()
        {
            DrugFees = new HashSet<DrugFee>();
        }
        [Key]
        [Display(Name = "药物记录ID")]
        public virtual Guid InPatientDrugRecordID { get; set; }
        [Display(Name = "原HIS医嘱ID")]
        public virtual Guid? Origin_ORDER_ID { get; set; }
        [Display(Name = "住院病历ID")]
        public virtual Guid InPatientID { get; set; }
        [Display(Name = "原HIS抗菌药物等级")]
        public virtual int? Origin_KSSDJ { get; set; }
        [Display(Name = "原HIS执行科室")]
        public virtual int Origin_EXEC_DEPT { get; set; }
        [Display(Name = "原HIS开医属医生")]
        public virtual int Origin_ORDER_DOC { get; set; }
        [Display(Name = "原HIS药物CJID")]
        public virtual int Origin_CJID { get; set; }
        [Display(Name = "药物品名")]
        public virtual string ProductName { get; set; }
        [Display(Name = "基本药物")]
        public virtual bool IsEssential { get; set; }
        [Display(Name = "药物剂型")]
        public virtual string DosageForm { get; set; }
        [Display(Name = "DDD值")]
        public virtual Decimal DDD { get; set; }
        [Display(Name = "原HIS药物用法")]
        public virtual string Origin_ORDER_USAGE { get; set; }
        public virtual InPatient InPatient { get; set; }
        public virtual HashSet<DrugFee> DrugFees { get; set; }

        //扩展方法
        #region 抗菌药物

        public bool IsAntibioticDrug
        {
            get
            {
                return this.Origin_KSSDJ.HasValue && this.Origin_KSSDJ.Value >= 1 && this.Origin_KSSDJ.Value <= 3;
            }
        }

        public Decimal AntibioticCost(DateTime startTime, DateTime endTime)
        {
            return this.IsAntibioticDrug 
                ? this.DrugFees.Sum(d => d.ActualPriceInDuration(startTime, endTime))
                : 0;
        }
        /// <summary>
        /// 返回以执行科室为基础的抗菌药物ID及抗菌药物金额.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>DepartmentCost.</returns>
        internal DepartmentCost AntibioticDepartmentCost(DateTime startTime, DateTime endTime)
        {
            var result = new DepartmentCost
            {
                DepartmentID = this.Origin_EXEC_DEPT,
                DrugCJID = this.Origin_CJID,
                Cost = AntibioticCost(startTime, endTime)
            };
            return result;
        }
        #endregion
        #region 总费用
        internal Decimal DrugCost(DateTime startTime, DateTime endTime)
        {
            return this.DrugFees.Sum(a => a.ActualPrice);
        }
        #endregion
    }
}
