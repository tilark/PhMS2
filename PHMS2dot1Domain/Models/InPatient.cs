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
using ClassViewModelToDomain;

namespace PhMS2dot1Domain.Models
{
    [Table("InPatients")]
    public class InPatient
    {
        #region Domain

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
        #endregion

        #region 扩展方法

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
            var result = this.InPatientDrugRecords.GroupBy(i => i.Origin_EXEC_DEPT).Select(g => new AntibioticDepartmentPerson { DepartmentID = g.Key, preStartTimeCost = g.Sum(a => a.AntibioticCost(this.OutDate.Value, startTime)), preEndTimeCost = g.Sum(a => a.AntibioticCost(this.OutDate.Value, endTime)), Cost = g.Sum(a => a.AntibioticCost(startTime, endTime)), IsAntibiotic = true }).ToList();
            return result;
        }

        public Decimal AntibioticCost(DateTime startTime, DateTime endTime)
        {
            Decimal result = 0;
            result = this.InPatientDrugRecords.Sum(i => i.AntibioticCost(startTime, endTime));
            return result;
        }

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
        #region 全部药物
        /// <summary>
        /// 获取科室的全部药物费用
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<DepartmentCost> GetDepartmentTotalDrugCost(DateTime startTime, DateTime endTime)
        {
            var result = this.InPatientDrugRecords.Select(i => i.GetDepartmentTotalDrugCost(startTime, endTime)).GroupBy(a => new { a.DepartmentID, a.DrugCJID }).Select(g => new DepartmentCost { DepartmentID = g.Key.DepartmentID, Cost = g.Sum(b => b.Cost), DrugCJID = g.Key.DrugCJID }).ToList();
            return result;
        }

       
        public Decimal GetTotalDrugCost(DateTime startTime, DateTime endTime)
        {
            Decimal result = Decimal.Zero;
            try
            {
                result = this.InPatientDrugRecords.Sum(i => i.DrugCost(startTime, endTime));
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("读取数据出错! {0}", e.Message));
            }
            return result;
        }


        #endregion
        #region Ddd        
        /// <summary>
        /// 取定时间段内的Ddd总值.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>Decimal.</returns>
        public Decimal TotalDddInDuration(DateTime startTime, DateTime endTime)
        {

            return this.InPatientDrugRecords.Sum(i => i.TotalDddInDuration(startTime, endTime));

        }
        /// <summary>
        /// 取定时间段内的特殊抗菌药物的DDD值.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>Decimal.</returns>
        public Decimal SpecialDddInDuration(DateTime startTime, DateTime endTime)
        {
            return this.InPatientDrugRecords.Sum(i => i.TotalSpecialDddInDuration(startTime, endTime));
        }
        /// <summary>
        /// 获取科室 & Ddd总值的集合
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;DepartmentDdd&gt;.</returns>
        public List<DepartmentDdd> GetDepartmentDddList(DateTime startTime, DateTime endTime)
        {
            var result = this.InPatientDrugRecords.Select(i => i.GetDepartmentDdd(startTime, endTime)).GroupBy(a => a.DepartmentID).Select(g => new DepartmentDdd { DepartmentID = g.Key, Ddd = g.Sum(b => b.Ddd) }).ToList();
            return result;
        }
        /// <summary>
        /// 获取特殊抗菌药物的科室与Ddd总消耗
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<DepartmentDdd> GetDepartmentSpecialDddList(DateTime startTime, DateTime endTime)
        {
            var result = this.InPatientDrugRecords.Select(i => i.GetDepartmentSpecialDdd(startTime, endTime)).GroupBy(a => a.DepartmentID).Select(g => new DepartmentDdd { DepartmentID = g.Key, Ddd = g.Sum(b => b.Ddd) }).ToList();
            return result;
        }
        #endregion

        #region 住院天数与出院人数
        /// <summary>
        /// 该患者的住院天数
        /// </summary>
        public int InHospitalDays
        {
            get
            {
                return OutDate.HasValue ? OutDate.Value.Subtract(InDate).Days : 0;
            }
        }
        /// <summary>
        /// 该患者在取定时间段内出院的人数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int OutHospitalPerson(DateTime startTime, DateTime endTime)
        {
            return this.OutDate.HasValue && this.OutDate.Value >= startTime && this.OutDate.Value < endTime
                ? 1
                : 0;
        }
        #endregion
        #region 药物种类
        /// <summary>
        /// 该住院病例信息包含的药物种类需计数为+1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        internal List<int> DrugCategoryNumberPositiveList(DateTime startTime, DateTime endTime, EnumDrugCategory drugCategory)
        {
            List<int> result = new List<int>();
            //获取在startTime之前的基本药物种类总金额==0的集合，
            //并且获取在endTime之前的基本药物种类总金额 >0的集合
            try
            {
                var preStartTimeList = this.InPatientDrugRecords.Select(opp => opp.GetDrugCategoryMessage(this.OutDate.Value, startTime, drugCategory)).ToList()
                .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
                var preStartTimeGreaterZeroList = InPatientDrugRecords.Select(opp => opp.GetDrugCategoryMessage(this.OutDate.Value, startTime, drugCategory)).ToList()
                    .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();

                var preEndTimeList = InPatientDrugRecords.Select(opp => opp.GetDrugCategoryMessage(this.OutDate.Value, endTime, drugCategory)).ToList()
                    .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
                //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
                if (preStartTimeGreaterZeroList.Count > 0 && preEndTimeList.Count > 0)
                {
                    //返回空，计数0


                }
                else if (preStartTimeList.Count > 0 && preEndTimeList.Count > 0)
                {
                    result = preEndTimeList.Intersect(preEndTimeList).ToList();

                }
                else if (preStartTimeList.Count == 0 && preEndTimeList.Count > 0)
                {
                    result = preEndTimeList;
                }
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据操作出错! {0}", e.Message));
            }

            return result;
        }


        /// <summary>
        /// 该挂号信息包含的药物种类需计数为-1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        internal List<int> DrugCategoryNumberNegativeList(DateTime startTime, DateTime endTime, EnumDrugCategory drugCategory)
        {
            List<int> result = new List<int>();
            //获取在startTime之前的基本药物种类总金额==0的集合，
            //并且获取在endTime之前的基本药物种类总金额 >0的集合
            try
            {
                var preStartTimeList = this.InPatientDrugRecords.Select(opp => opp.GetDrugCategoryMessage(this.OutDate.Value, startTime, drugCategory)).ToList()
               .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
                var preEndTimeList = InPatientDrugRecords.Select(opp => opp.GetDrugCategoryMessage(this.OutDate.Value, endTime, drugCategory)).ToList()
                    .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
                //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
                if (preStartTimeList.Count > 0 && preEndTimeList.Count > 0)
                {
                    result = preEndTimeList.Intersect(preEndTimeList).ToList();
                }
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据操作出错! {0}", e.Message));
            }
           
            return result;
        }
        #endregion
        #region 基本药物
        /// <summary>
        /// 获取科室及基本药物费集合
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<DepartmentCost> GetDepartmentEssentialDrugCost(DateTime startTime, DateTime endTime)
        {
            var result = this.InPatientDrugRecords.Select(i => i.GetDepartmentEssentialDrugCost(startTime, endTime)).GroupBy(a => new { a.DepartmentID , a.DrugCJID} ).Select(g => new DepartmentCost { DepartmentID = g.Key.DepartmentID, Cost = g.Sum(b => b.Cost), DrugCJID = g.Key.DrugCJID}).ToList();
            return result;
        }
        #endregion
        #endregion

    }
}
