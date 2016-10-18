using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PHMS2Domain.Models
{
    public partial class Registers
    {
        #region 抗菌药

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
            
            if(preStartTimeCost > 0 && preEndTimeCost == 0){
                num = -1;
            }
            else if(preStartTimeCost  == 0 && preEndTimeCost > 0)
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
        /// <returns>返回1，0.</returns>
        /// <remarks>
        /// 总金额在开始时间之前大于0并且在结束时间之前等于0，计数为-1；
        /// 其他情况计数为0；
        /// </remarks>
        public int AntibioticPersonNegative(DateTime startTime, DateTime endTime)
        {
            var preStartTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.AntibioticCost(this.ChargeTime, startTime));
            var preEndTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.AntibioticCost(this.ChargeTime, endTime));         

            return (preStartTimeCost > 0 && preEndTimeCost == 0) ? -1 : 0;
        }
        public int AntibioticPersonWeight(DateTime startTime, DateTime endTime)
        {
            bool pre = IsMatchAntibiotic(this.ChargeTime, startTime);
            bool whole = IsMatchAntibiotic(this.ChargeTime, endTime);

            if (pre == true && whole == true)
                return 0;
            else if (pre == true && whole == false)
                return -1;
            else if (pre = false && whole == true)
                return 1;
            else
                return 0;
        }
        public bool IsContainAntibiotic()
        {
            //对应的所有Prescriptions中至少有一个含有抗菌药物，即认为含有抗菌药物
            return OutPatientPrescriptions.Any(opp => opp.IsContainAntibiotic());
        }
        public bool IsContainAntibiotic(DateTime startTime,DateTime endTime)
        {
            return OutPatientPrescriptions.Any(opp => opp.IsContainAntibiotic(startTime, endTime));
                
        }
        public bool IsMatchAntibiotic(DateTime startTime, DateTime endTime)
        {
            return OutPatientPrescriptions.Sum(c => c.AntibioticCost(startTime, endTime)) == 0;
        }
        /// <summary>
        /// 抗菌药物费用
        /// </summary>
        /// <returns>Decimal.</returns>
        public Decimal AntibioticCost()
        {
            Decimal cost = 0;
            cost = OutPatientPrescriptions.Sum(opp => opp.AntibioticCost());

            return cost;
        }
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
        /// 获取该挂号信息包含的所有抗菌药物种类数量
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>System.Int32.</returns>
        public int AntibioticCategoryNumber(DateTime startTime, DateTime endTime)
        {
            return AntibioticCategoryNumberList(startTime, endTime).Count;
        }
        #endregion
        #region 药物费用        
        /// <summary>
        /// 病人药品总费用.
        /// </summary>
        /// <returns>Decimal.</returns>
        public Decimal DrugCost()
        {
            return OutPatientPrescriptions.Sum(opp => opp.DrugCost());
        }
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
            //if (DateTime.Compare(ChargeTime, startTime) >= 0 && DateTime.Compare(ChargeTime, endTime) < 0)
            //{
            //    cost = DrugCost();
            //}
            cost = OutPatientPrescriptions.Sum(opp => opp.DrugCost(startTime, endTime));

            return cost;
        }
        #endregion
        #region 药物种类
        /// <summary>
        /// 该挂号信息包含的基本药物种类需计数为+1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        internal List<string> DrugCategoryNumberPositiveList(DateTime startTime, DateTime endTime, EnumDrugCategories drugCategory)
        {
            List<string> result = new List<string>();
            //获取在startTime之前的基本药物种类总金额==0的集合，
            //并且获取在endTime之前的基本药物种类总金额 >0的集合

            var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetDrugCategoryMessage(this.ChargeTime, startTime, drugCategory)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
            var preStartTimeGreaterZeroList = OutPatientPrescriptions.SelectMany(opp => opp.GetDrugCategoryMessage(this.ChargeTime, startTime, drugCategory)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetDrugCategoryMessage(this.ChargeTime, endTime, drugCategory)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
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
            return result;
        }
        /// <summary>
        /// 该挂号信息包含的基本药物种类需计数为-1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        internal List<string> DrugCategoryNumberNegativeList(DateTime startTime, DateTime endTime, EnumDrugCategories drugCategory)
        {
            List<string> result = new List<string>();
            //获取在startTime之前的基本药物种类总金额==0的集合，
            //并且获取在endTime之前的基本药物种类总金额 >0的集合

            var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, endTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
            //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
            if (preStartTimeList.Count > 0 && preEndTimeList.Count > 0)
            {
                result = preEndTimeList.Intersect(preEndTimeList).ToList();

            }
            return result;
        }
        #endregion
        #region 基本药物信息        
        /// <summary>
        /// 该挂号信息包含的基本药物种类需计数为+1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> EssentialDrugCategoryNumberPositiveList(DateTime startTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            //获取在startTime之前的基本药物种类总金额==0的集合，
            //并且获取在endTime之前的基本药物种类总金额 >0的集合
            
            var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
            var preStartTimeGreaterZeroList = OutPatientPrescriptions.SelectMany(opp => opp.GetAllDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, endTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
            if (preStartTimeGreaterZeroList.Count > 0 && preEndTimeList.Count > 0)
            {
                //返回空，计数0

            }
            if (preStartTimeList.Count > 0 && preEndTimeList.Count > 0)
            {
                result = preEndTimeList.Intersect(preEndTimeList).ToList();

            }           
            else if (preStartTimeList.Count == 0 && preEndTimeList.Count > 0)
            {
                result = preEndTimeList;
            }
            return result;
        }
        /// <summary>
        /// 该挂号信息包含的基本药物种类需计数为-1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> EssentialDrugCategoryNumberNegativeList(DateTime startTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            //获取在startTime之前的基本药物种类总金额==0的集合，
            //并且获取在endTime之前的基本药物种类总金额 >0的集合

            var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetEssentialDrugMessage(this.ChargeTime, endTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
            //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
            if (preStartTimeList.Count >0 && preEndTimeList.Count > 0)
            {
                result = preEndTimeList.Intersect(preEndTimeList).ToList();

            }            
            return result;
        }
        #endregion
        #region 全部药物信息（不区分抗菌或基本药物）
        /// <summary>
        /// 该挂号信息包含的所有药物种类需计数为+1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> AllDrugCategoryNumberPositiveList(DateTime startTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            //获取在startTime之前的所有药物种类总金额==0的集合，
            //并且获取在endTime之前的所有药物种类总金额 >0的集合

            var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetAllDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
            var preStartTimeGreaterZeroList = OutPatientPrescriptions.SelectMany(opp => opp.GetAllDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetAllDrugMessage(this.ChargeTime, endTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();
            //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
            if(preStartTimeGreaterZeroList.Count > 0 && preEndTimeList.Count > 0)
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
            return result;
        }
        /// <summary>
        /// 该挂号信息包含的所有药物种类需计数为-1的药品种类集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> AllDrugCategoryNumberNegativeList(DateTime startTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            //获取在startTime之前的所有药物种类总金额==0的集合，
            //并且获取在endTime之前的所有药物种类总金额 >0的集合

            var preStartTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetAllDrugMessage(this.ChargeTime, startTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) > 0).Select(s => s.Key).ToList();

            var preEndTimeList = OutPatientPrescriptions.SelectMany(opp => opp.GetAllDrugMessage(this.ChargeTime, endTime)).ToList()
                .GroupBy(g => g.ProductNumber).Where(w => w.Sum(su => su.Cost) == 0).Select(s => s.Key).ToList();
            //preStartTimeList  preEndTimeList 的交集，即为该挂号信息中有效的药品种类
            if (preStartTimeList.Count > 0 && preStartTimeList.Count > 0)
            {
                result = preEndTimeList.Intersect(preEndTimeList).ToList();
            }
            return result;
        }
        #endregion
        #region 挂号人数
        /// <summary>
        /// 根据挂号类型返回在取定时间段内的挂号人数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="OutPatientCategory">就诊类型</param>
        /// <returns>System.Int32.</returns>
        public int RegisterPerson(DateTime startTime, DateTime endTime, string OutPatientCategory)
        {
            int num = (DateTime.Compare(ChargeTime, startTime) >= 0 && DateTime.Compare(ChargeTime, endTime) < 0)
                ? String.IsNullOrEmpty(OutPatientCategory)
                    ? 1
                    : String.Compare(RegisterCategories.RegisterCategoryName, OutPatientCategory) == 0
                        ? 1
                        : 0
                : 0;
            return num;
        }
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
        /// <summary>
        /// 返回取消挂号人数.区分就诊类型
        /// </summary>
        /// <param name="starTime">The star time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="OutPatientCategory">就诊类型（门诊/急诊）</param>
        /// <returns>System.Int32.</returns>
        public int CancelRegisterPerson(DateTime startTime, DateTime endTime, string OutPatientCategory)
        {
            int num = CancelRegisterTime.HasValue
                    ? (DateTime.Compare(CancelRegisterTime.Value, startTime) >= 0 && DateTime.Compare(CancelRegisterTime.Value, endTime) < 0)
                        ? String.IsNullOrEmpty(OutPatientCategory)
                            ? -1
                            : String.Compare(RegisterCategories.RegisterCategoryName, OutPatientCategory) == 0
                                ? -1
                                : 0
                        : 0
                    : 0;

            return num;
        }
        /// <summary>
        /// 返回取消挂号人数
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>System.Int32.</returns>
        public int CancelRegisterPerson(DateTime startTime, DateTime endTime)
        {
            int num = CancelRegisterTime.HasValue
                    ? (DateTime.Compare(CancelRegisterTime.Value, startTime) >= 0 && DateTime.Compare(CancelRegisterTime.Value, endTime) < 0)
                        ? -1
                        : 0
                    : 0;

            return num;
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
        /// 使用药物人数（正值）.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>System.Int32.</returns>
        /// <remarks>
        /// 药物总金额在取定时间段开始时间之前等于0且在结束时间之前大于0，计数1
        /// 其余计数0
        /// </remarks>
        public int DrugPersonPositive(DateTime startTime, DateTime endTime)
        {
            var preStartTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.DrugCost(this.ChargeTime, startTime));
            var preEndTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.DrugCost(this.ChargeTime, endTime));
            return (preStartTimeCost == 0 && preEndTimeCost > 0) ? 1 : 0;
        }

        /// <summary>
        /// 使用药物人数（负值）.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>System.Int32.</returns>
        /// <remarks>
        /// 药物总金额在取定时间段开始时间之前大于0且在结束时间之前等于0，计数-1
        /// 其余计数0
        /// </remarks>
        public int DrugPersonNegative(DateTime startTime, DateTime endTime)
        {
            var preStartTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.DrugCost(this.ChargeTime, startTime));
            var preEndTimeCost = this.OutPatientPrescriptions.Sum(opp => opp.DrugCost(this.ChargeTime, endTime));

            return (preStartTimeCost > 0 && preEndTimeCost == 0) ? -1 : 0;
        }
        #endregion
        #region 取定时间段内挂号信息
        public bool IsInDuration(DateTime startTime, DateTime endTime)
        {
            return OutPatientPrescriptions.Any(opp => opp.IsInDuration(startTime, endTime));
        }
        #endregion



    }
}