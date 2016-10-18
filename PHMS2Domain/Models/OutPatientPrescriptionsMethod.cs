using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2Domain;
using ClassViewModelToDomain;

namespace PHMS2Domain.Models
{
    public partial class OutPatientPrescriptions
    {
        #region 抗菌药操作
        public bool IsContainAntibiotic()
        {
            bool isContainAntibiotic = false;

            isContainAntibiotic = OutPatientPrescriptionDetails.Any(oppd => oppd.IsContainAntibiotic());
            return isContainAntibiotic;
        }
        public bool IsContainAntibiotic(DateTime startTime, DateTime endTime)
        {
            bool isContainAntibiotic = false;
            if(this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                isContainAntibiotic = OutPatientPrescriptionDetails.Any(oppd => oppd.IsContainAntibiotic());
            }
            return isContainAntibiotic;
        }
        /// <summary>
        /// 处方单包含抗菌药物的费用
        /// </summary>
        /// <returns>Decimal.</returns>
        public Decimal AntibioticCost()
        {
            return OutPatientPrescriptionDetails.Sum(opp => opp.AntibioticCost());
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
            //if (DateTime.Compare(ChargeTime, startTime) >= 0 && DateTime.Compare(ChargeTime, endTime) < 0)
            if(this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                cost = AntibioticCost();
            }
            return cost;
        }
        public List<DrugCost> GetAntibioticCost()
        {
            return OutPatientPrescriptionDetails.Where(oppd => oppd.IsContainAntibiotic()).Select(opp => new DrugCost
            {
                ProductNumber = opp.DrugMaintenances.ProductNumber,
                ProductName = opp.DrugMaintenances.ProductName,
                Cost = opp.AntibioticCost(),
                IsAntibiotic = true

            }).ToList();
        }

        public List<string> AntibioticCategoryNumberList()
        {
            return OutPatientPrescriptionDetails.Select(opp => opp.AntibioticCategoryNumber).ToList();
        }
        public List<string> AntibioticCategoryNumberList(DateTime startTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            result = this.ChargeTime >= startTime && this.ChargeTime < endTime
                ? OutPatientPrescriptionDetails.Select(opp => opp.AntibioticCategoryNumber).ToList()
                : result;
            return result;
        }
        #endregion

        #region  药物费用

        public List<DrugCost> GetDrugCost()
        {
            return OutPatientPrescriptionDetails.Select(opp => new DrugCost
            {
                ProductNumber = opp.DrugMaintenances.ProductNumber,
                ProductName = opp.DrugMaintenances.ProductName,
                Cost = opp.DrugCost(),
                IsAntibiotic = opp.IsContainAntibiotic()
            }).ToList();
        }
        public List<DrugCost> GetDrugCost(DateTime startTime, DateTime endTime)
        {
            if (this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                return OutPatientPrescriptionDetails.Select(opp => new DrugCost
                {
                    ProductNumber = opp.DrugMaintenances.ProductNumber,
                    ProductName = opp.DrugMaintenances.ProductName,
                    Cost = opp.DrugCost(),
                    IsAntibiotic = opp.IsContainAntibiotic()
                }).ToList();
            }
            else
            {
                return null;
            }

        }
        public List<DrugDoctorDepartmentCost> GetDrugDoctorDepartmentCost(string productNumber)
        {
            return OutPatientPrescriptionDetails.Where(oppd => oppd.DrugMaintenances.ProductNumber == productNumber)
                .Select(c => new DrugDoctorDepartmentCost
                {
                    Doctor = Doctors.DoctorName,
                    Department = Departments.DepartmentName,
                    Cost = c.Quantity * c.DrugMaintenances.UnitCost
                }).ToList();
        }


        /// <summary>
        /// 处方单的部金额
        /// </summary>
        /// <returns>Decimal.</returns>
        public Decimal DrugCost()
        {
            return OutPatientPrescriptionDetails.Sum(oppd => oppd.DrugCost());
        }
        /// <summary>
        /// 取定时间范围内的处方单总金额
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>Decimal.</returns>
        public Decimal DrugCost(DateTime startTime, DateTime endTime)
        {
            Decimal cost = 0;
            //在选定时间段内
            if (DateTime.Compare(ChargeTime, startTime) >= 0 && DateTime.Compare(ChargeTime, endTime) <= 0)
            {
                cost = DrugCost();
            }
            return cost;
        }
        #endregion

        #region 药品种类

        /// <summary>
        /// 获得该处方包含的基本药物信息的集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;EssentialDrugMessage&gt;.</returns>
        internal List<DrugMessage> GetDrugCategoryMessage(DateTime startTime, DateTime endTime, EnumDrugCategories drugCategory)
        {
            List<DrugMessage> result = new List<DrugMessage>();
            if (this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                result = OutPatientPrescriptionDetails.Select(oop => oop.GetDrugCategoryMessage(drugCategory)).ToList();
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
        internal List<EssentialDrugMessage> GetEssentialDrugMessage(DateTime startTime, DateTime endTime)
        {
            List<EssentialDrugMessage> result = new List<EssentialDrugMessage>();
            if(this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                result = OutPatientPrescriptionDetails.Select(oop => oop.GetEssentialDrugMessage()).ToList();
            }
            return result;
        }
        #endregion
        #region 全部药物
        /// <summary>
        /// 获得该处方包含的所有药物信息的集合.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;EssentialDrugMessage&gt;.</returns>
        internal List<AllDrugMessage> GetAllDrugMessage(DateTime startTime, DateTime endTime)
        {
            List<AllDrugMessage> result = new List<AllDrugMessage>();
            if (this.ChargeTime >= startTime && this.ChargeTime < endTime)
            {
                result = OutPatientPrescriptionDetails.Select(oop => oop.GetAllDrugMessage()).ToList();
            }
            return result;
        }
        #endregion
        #region 退费
        /// <summary>
        /// 如果该张处方单金额小于0,则为退费处方.
        /// </summary>
        /// <returns><c>true</c> if this instance is refund; otherwise, <c>false</c>.</returns>
        public bool IsRefund()
        {
            return (DrugCost() < Decimal.Zero) ? true : false;
        }
        /// <summary>
        /// 返回退费处方的挂号信息ID
        /// </summary>
        /// <returns>Guid.</returns>
        public Guid GetRefundRegisterID()
        {
            return IsRefund() ? RegisterId : Guid.Empty;
        }
        #endregion

        #region 取定时间段内
        public bool IsInDuration(DateTime startTime, DateTime endTime)
        {
            return this.ChargeTime >= startTime && this.ChargeTime < endTime;
        }
        #endregion

    }
}