using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PhMS2dot1Domain.ViewModels;

namespace PhMS2dot1Domain.Models
{
    [Table("InPatients")]
    public class InPatient
    {
        public InPatient()
        {
            InPatientDrugRecords = new HashSet<InPatientDrugRecord>();
        }
        [Key]
        [Display(Name = "住院病历ID")]
        public virtual Guid InPatientID { get; set; }

        [Display(Name = "原HIS住院病历ID")]
        public virtual Guid? Origin_INPATIENT_ID { get; set; }

        [Display(Name = "病人信息ID")]
        public virtual Guid PatientID { get; set; }

        [Display(Name = "住院号")]
        public virtual String CaseNumber { get; set; }

        [Display(Name = "住院次数")]
        public virtual int Times { get; set; }

        [Display(Name = "入院时间")]
        public DateTime InDate { get; set; }
        [Display(Name = "入院科室（原HIS）")]

        public virtual long Origin_IN_DEPT { get; set; }
        [Display(Name = "出院时间")]
        public DateTime? OutDate { get; set; }

        [Display(Name = "当前科室（原HIS）")]
        public virtual long Origin_DEPT_ID { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual ICollection<InPatientDrugRecord> InPatientDrugRecords { get; set; }

        //扩展方法
        #region 抗菌药物

        /// <summary>
        /// 住院患者抗菌使用人数计数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>返回1，0.</returns>
        /// <remarks>
        /// 抗菌药物处方表总金额在开始时间之前为0并且在结束时间之前大于0，计数为1；
        /// 其他情况计数为0；
        /// </remarks>
        public int AntibioticPersonPositive(DateTime startTime, DateTime endTime)
        {
            var preStartTimeCost = this.InPatientDrugRecords.Sum(opp => opp.AntibioticCost(this.OutDate.Value, startTime));
            var preEndTimeCost = this.InPatientDrugRecords.Sum(opp => opp.AntibioticCost(this.OutDate.Value, endTime));
            return (preStartTimeCost == 0 && preEndTimeCost > 0) ? 1 : 0;
        }
        /// <summary>
        /// 住院患者抗菌使用人数计数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>返回1，0.</returns>
        /// <remarks>
        /// 总金额在开始时间之前大于0并且在结束时间之前等于0，计数为-1；
        /// 其他情况计数为0；
        /// </remarks>
        public int AntibioticPersonNegative(DateTime startTime, DateTime endTime)
        {
            var preStartTimeCost = this.InPatientDrugRecords.Sum(opp => opp.AntibioticCost(this.OutDate.Value, startTime));
            var preEndTimeCost = this.InPatientDrugRecords.Sum(opp => opp.AntibioticCost(this.OutDate.Value, endTime));

            return (preStartTimeCost > 0 && preEndTimeCost == 0) ? -1 : 0;
        }
        /// <summary>
        /// 抗菌药物科室及使用抗菌药物人数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;DepartmentPerson&gt; 以执行科室为组，统计出在该科室中使用抗菌药物的情况.</returns>
        /// <remarks>获得在preStartTime时的抗菌药总费用，获得在preEndTime之前的抗菌药物总费用，根据两者费用的情况计算人数</remarks>
        public List<AntibioticDepartmentPerson> AntibioticDepartmentPersonList(DateTime startTime, DateTime endTime)
        {
            var result = this.InPatientDrugRecords.GroupBy(i => i.Origin_EXEC_DEPT).Select(g => new AntibioticDepartmentPerson { DepartmentID = g.Key, preStartTimeCost = g.Sum(a => a.AntibioticCost(this.OutDate.Value , startTime)), preEndTimeCost = g.Sum(a => a.AntibioticCost(this.OutDate.Value, endTime)), Cost = g.Sum(a => a.AntibioticCost(startTime, endTime)), IsAntibiotic = true }).ToList();
            return result;
        }

        public Decimal AntibioticCost(DateTime startTime, DateTime endTime)
        {
            Decimal result = 0;
            result = this.InPatientDrugRecords.Sum(i => i.AntibioticCost(startTime, endTime));
            return result;
        }
        #endregion
        #region 药物费用
        /// <summary>
        /// 抗菌药物科室及使用抗菌药物人数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;DepartmentPerson&gt; 以执行科室为组，统计出在该科室中使用抗菌药物的情况.</returns>
        /// <remarks>获得在preStartTime时的抗菌药总费用，获得在preEndTime之前的抗菌药物总费用，根据两者费用的情况计算人数</remarks>
        public List<DepartmentPerson> DepartmentPersonList(DateTime startTime, DateTime endTime)
        {
            var result = this.InPatientDrugRecords.GroupBy(i => i.Origin_EXEC_DEPT).Select(g => new DepartmentPerson { DepartmentID = g.Key, preStartTimeCost = g.Sum(a => a.AntibioticCost(this.OutDate.Value, startTime)), preEndTimeCost = g.Sum(a => a.AntibioticCost(this.OutDate.Value, endTime)), Cost = g.Sum(a => a.AntibioticCost(startTime, endTime)) }).ToList();
            return result;
        }

        #endregion
    }
}
