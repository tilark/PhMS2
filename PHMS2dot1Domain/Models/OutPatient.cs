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
    [Table("OutPatients")]

    public class OutPatient
    {
        #region Domain

        public OutPatient()
        {
            OutPatientPrescriptions = new List<OutPatientPrescription>();
        }
        [Key]
        [Display(Name = "门诊病例ID")]
        public virtual Guid OutPatientID { get; set; }

        [Display(Name = "原HIS门诊挂号ID")]
        public virtual Guid? Origin_GHXXID { get; set; }

        [Display(Name = "挂号类别")]
        public virtual int? Origin_GHLB { get; set; }

        [Display(Name = "挂号时间")]
        public virtual DateTime ChargeTime { get; set; }

        [Display(Name = "取消挂号时间")]
        public virtual DateTime? CancelChargeTime { get; set; }

        [Display(Name = "病人信息ID")]
        public virtual Guid PatientID { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual List<OutPatientPrescription> OutPatientPrescriptions { get; set; }
        #endregion
        #region 扩展方法

        #region 抗菌药物

        /// <summary>
        /// 取定时间段内抗菌药物费用.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>Decimal.</returns>
        /// <remarks>
        /// 取定的时间段应用处方表中的时间决定
        /// </remarks>
        public Decimal AntibioticCost(DateTime startTime, DateTime endTime)
        {
            Decimal cost = 0;
            //在选定时间段内,时间由处方表中的时间决定
            //if (DateTime.Compare(ChargeTime, startTime) >= 0 && DateTime.Compare(ChargeTime, endTime) < 0)
            //{
            //    cost = AntibioticCost();
            //}
            cost = OutPatientPrescriptions.Sum(opp => opp.AntibioticCost(startTime, endTime));

            return cost;
        }
        /// <summary>
        /// 使用药物人数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>System.Int32.</returns>
        /// <remarks>
        /// 药物总金额在取定时间段开始时间之前大于0且在结束时间之前等于0，计数-1
        /// 药物总金额在取定时间段开始时间之前等于0且在结束时间之前大于0，计数1
        /// 其余计数0
        /// </remarks>
        public int DrugPerson(DateTime startTime, DateTime endTime)
        {
            int num = 0;
            var preStartTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.DrugCost(this.ChargeTime, startTime));
            var preEndTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.DrugCost(this.ChargeTime, endTime));

            if (preStartTimeCost > 0 && preEndTimeCost == 0)
            {
                num = -1;
            }
            else if (preStartTimeCost == 0 && preEndTimeCost > 0)
            {
                num = 1;
            }

            return num;
        }
        /// <summary>
        /// 门诊与急诊抗菌使用人数计数.
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
            var preStartTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.AntibioticCost(this.ChargeTime, startTime));
            var preEndTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.AntibioticCost(this.ChargeTime, endTime));
            return (preStartTimeCost == 0 && preEndTimeCost > 0) ? 1 : 0;
        }
        /// <summary>
        /// 门诊与急诊抗菌使用人数计数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>返回1，0，-1.</returns>
        /// <remarks>
        /// 抗菌药物处方表总金额在开始时间之前为0并且在结束时间之前大于0，计数为1；
        /// 总金额在开始时间之前大于0并且在结束时间之前等于0，计数为-1；
        /// 其他情况计数为0；
        /// </remarks>
        public int AntibioticPerson(DateTime startTime, DateTime endTime)
        {
            int num = 0;
            var preStartTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.AntibioticCost(this.ChargeTime, startTime));
            var preEndTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.AntibioticCost(this.ChargeTime, endTime));

            if (preStartTimeCost > 0 && preEndTimeCost == 0)
            {
                num = -1;
            }
            else if (preStartTimeCost == 0 && preEndTimeCost > 0)
            {
                num = 1;
            }

            return num;
        }
        #endregion

        #region 挂号人数
        /// <summary>
        /// 取定时间段内的挂号人数，包含所有就诊类型
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>System.Int32.</returns>
        public int RegisterPerson(DateTime startTime, DateTime endTime)
        {
            int num = (DateTime.Compare(ChargeTime, startTime) >= 0
                && DateTime.Compare(ChargeTime, endTime) < 0)
                ? 1
                : 0;
            return num;
        }
        #endregion

        #region 药物种类
        /// <summary>
        /// 获取该挂号信息包含的所有抗菌药物编码列表.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>HashSet&lt;System.String&gt;.</returns>
        /// <remarks>取定时间段的抗菌药费用是否大于0，大于0说明需计数，再取对应的抗菌药品种</remarks>
        public List<string> AntibioticCategoryNumberList(DateTime startTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            result = this.AntibioticPersonPositive(startTime, endTime) > 0
                ? OutPatientPrescriptions.SelectMany(opp => opp.AntibioticCategoryNumberList(startTime, endTime)).Distinct().ToList()
                : result;
            return result;
        }

        /// <summary>
        /// 该挂号信息包含的基本药物种类需计数为+1的药品种类集合.
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
                var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetDrugCategoryMessage(this.ChargeTime, startTime, drugCategory)).ToList()
                .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
                var preStartTimeGreaterZeroList = OutPatientPrescriptions.SelectMany(opp => opp.GetDrugCategoryMessage(this.ChargeTime, startTime, drugCategory)).ToList()
                    .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
                var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetDrugCategoryMessage(this.ChargeTime, endTime, drugCategory)).ToList()
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
        /// 该挂号信息包含的基本药物种类需计数为-1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        internal List<int> DrugCategoryNumberNegativeList(DateTime startTime, DateTime endTime, EnumDrugCategory drugCategory)
        {
            List<int> result = new List<int>();
            //获取在startTime之前的基本药物种类总金额==0的集合，
            //并且获取在endTime之前的基本药物种类总金额 >0的集合

            var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, endTime)).ToList()
                .GroupBy(g => g.ProductID).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
            //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
            if (preStartTimeList.Count > 0 && preEndTimeList.Count > 0)
            {
                result = preEndTimeList.Intersect(preEndTimeList).ToList();
            }
            return result;
        }


        #endregion

        #region 药物费用
        /// <summary>
        /// 取定时间内，病人药品总费用
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>Decimal.</returns>
        /// <remarks>
        /// 取定的时间段内由处方表中的时间决定
        /// </remarks>
        public Decimal DrugCost(DateTime startTime, DateTime endTime)
        {
            Decimal cost = 0;
            //在选定时间段内,由处方表中的时间决定
            
            cost = OutPatientPrescriptions.Sum(opp => opp.DrugCost(startTime, endTime));

            return cost;
        }

        //public List<DrugDoctorCost> GetDrugDoctorCost(DateTime startTime, DateTime endTime)
        //{
        //    var result = this.OutPatientPrescriptions.Select(i => i.GetDrugDoctorCost(startTime, endTime)).GroupBy(a => new { a.DoctorID, a.DepartmentID, a.ProductCJID }).Select(g => new DrugDoctorCost { DoctorID = g.Key })
        //}
        #endregion
        #endregion

    }

    //public class OutPatientComparer : IEqualityComparer<OutPatient>
    //{
    //    public bool Equals(OutPatient x, OutPatient y)
    //    {
    //        if (x != null && y != null & x.OutPatientID == y.OutPatientID)
    //            return true;
    //        else
    //            return false;
    //    }

    //    public int GetHashCode(OutPatient obj)
    //    {
    //        return obj.OutPatientID.GetHashCode();
    //    }
    //}
}
