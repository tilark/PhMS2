using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ClassViewModelToDomain;
using PhMS2dot1Domain.ViewModels;

namespace PhMS2dot1Domain.Models
{
    [Table("OutPatientPrescriptions")]
    public class OutPatientPrescription
    {
        #region Domain

        public OutPatientPrescription()
        {
            OutPatientDrugRecords = new HashSet<OutPatientDrugRecord>();
        }
        [Display(Name = "门诊处方ID")]
        public virtual Guid OutPatientPrescriptionID { get; set; }

        [Display(Name = "原HIS门诊处方ID")]
        public virtual Guid Origin_CFID { get; set; }

        [Display(Name = "门诊病例ID")]
        public virtual Guid OutPatientID { get; set; }

        [Display(Name = "收费时间")]
        public virtual DateTime ChargeTime { get; set; }

        [Display(Name = "原HIS科室代码")]
        public virtual int? Origin_KSDM { get; set; }

        [Display(Name = "原HIS医生代码")]
        public virtual long? Origin_YSDM { get; set; }

        public virtual OutPatient OutPatient { get; set; }

        public virtual ICollection<OutPatientDrugRecord> OutPatientDrugRecords { get; set; }

        #endregion
        #region //扩展方法


        #region 抗菌药物

        public List<DrugCost> GetAntibioticCost()
        {
            return OutPatientDrugRecords.Where(oppd => oppd.IsAntibiotic).Select(opp => new DrugCost
            {
                ProductCJID = opp.Origin_CJID.HasValue ? opp.Origin_CJID.Value : -1,
                ProductName = opp.ProductName,
                Cost = opp.AntibioticCost(),
                IsAntibiotic = true

            }).ToList();
        }
        public List<string> AntibioticCategoryNumberList(DateTime startTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            result = this.ChargeTime >= startTime && this.ChargeTime < endTime
                ? OutPatientDrugRecords.Select(opp => opp.AntibioticCategoryNumber).ToList()
                : result;
            return result;
        }

        public List<int> AntibioticCategoryIDList(DateTime startTime, DateTime endTime)
        {
            List<int> result = new List<int>();
            result = this.ChargeTime >= startTime && this.ChargeTime < endTime
                ? OutPatientDrugRecords.Select(opp => opp.AntibioticCategoryID).ToList()
                : result;
            return result;
        }
        /// <summary>
        /// 处方单包含抗菌药物的费用
        /// </summary>
        /// <returns>Decimal.</returns>
        public Decimal AntibioticCost()
        {
            return OutPatientDrugRecords.Sum(opp => opp.AntibioticCost());
        }
        /// <summary>
        /// 取定日期内处方单包含抗菌药物的费用
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>含抗菌药物处方单的总费用.</returns>
        public Decimal AntibioticCost(DateTime startTime, DateTime endTime)
        {
            Decimal cost = 0;
            //在选定时间段内
            if (this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                cost = AntibioticCost();
            }
            return cost;
        }
        #endregion
        #region 药物费用
        public List<DrugCost> GetDrugCost()
        {
            return OutPatientDrugRecords.Select(opp => new DrugCost
            {
                ProductCJID = opp.Origin_CJID.HasValue ? opp.Origin_CJID.Value : -1,
                ProductName = opp.ProductName,
                Cost = opp.DrugCost(),
                IsAntibiotic = opp.IsAntibiotic
            }).ToList();
        }

        public List<DrugDoctorDepartmentCost> GetDrugDoctorDepartmentCost(string productNumber)
        {
            return OutPatientDrugRecords.Where(oppd => oppd.Origin_CJID == int.Parse(productNumber))
                .Select(c => new DrugDoctorDepartmentCost
                {
                    DoctorID = this.Origin_YSDM.HasValue ? this.Origin_YSDM.Value : -1,
                    DepartmentID = this.Origin_KSDM.HasValue ? this.Origin_KSDM.Value : -1,
                    Cost = c.ActualPrice
                }).ToList();
        }
        #endregion

        #region 药品种类
        /// <summary>
        /// 获得该处方包含的基本药物信息的集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;EssentialDrugMessage&gt;.</returns>
        internal List<DrugMessage> GetDrugCategoryMessage(DateTime startTime, DateTime endTime, EnumDrugCategory drugCategory)
        {
            List<DrugMessage> result = new List<DrugMessage>();
            if (this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                result = OutPatientDrugRecords.Select(oop => oop.GetDrugCategoryMessage(drugCategory)).ToList();
            }
            return result;
        }
        #endregion

        #region 基本药物
        /// <summary>
        /// 获得该处方包含的基本药物信息的集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;EssentialDrugMessage&gt;.</returns>
        internal List<DrugMessage> GetEssentialDrugMessage(DateTime startTime, DateTime endTime)
        {
            List<DrugMessage> result = new List<DrugMessage>();
            if (this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                result = OutPatientDrugRecords.Select(oop => oop.GetEssentialDrugMessage()).ToList();
            }
            return result;
        }

        internal Decimal DrugCost(DateTime startTime, DateTime endTime)
        {
            Decimal cost = 0;
            //在选定时间段内
            if (DateTime.Compare(ChargeTime, startTime) >= 0 && DateTime.Compare(ChargeTime, endTime) <= 0)
            {
                cost = DrugCost();
            }
            return cost; ;
        }

        private decimal DrugCost()
        {
            return OutPatientDrugRecords.Sum(oppd => oppd.DrugCost()); ;
        }
        #endregion
        #endregion

    }
}
