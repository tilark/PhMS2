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
    [Table("InPatientDrugRecords")]
    public class InPatientDrugRecord
    {
        #region Domain

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
        public virtual long Origin_ORDER_DOC { get; set; }
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
        public virtual ICollection<DrugFee> DrugFees { get; set; }
        #endregion

        #region 扩展方法
        #region 抗菌药物

        public bool IsAntibioticDrug
        {
            get
            {
                return this.Origin_KSSDJ.HasValue && this.Origin_KSSDJ.Value >= 1 && this.Origin_KSSDJ.Value <= 3;
            }
        }

        public bool IsSpecialAntibioticDrug
        {
            get
            {
                return this.Origin_KSSDJ.HasValue && this.Origin_KSSDJ.Value == 3;
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
            return this.DrugFees.Sum(a => a.ActualPriceInDuration(startTime, endTime));
        }
        internal DepartmentCost GetDepartmentTotalDrugCost(DateTime startTime, DateTime endTime)
        {
            return new DepartmentCost
            {
                DepartmentID = this.Origin_EXEC_DEPT,
                Cost = DrugCost(startTime, endTime),
                DrugCJID = this.Origin_CJID
            };
        }


        #endregion
        #region Ddd        
        /// <summary>
        /// 取定时间段内的 DDD.
        /// </summary>
        /// <value>The total DDD.</value>
        internal Decimal TotalDddInDuration(DateTime startTime, DateTime endTime)
        {

            return this.DDD * this.DrugFees.Sum(d => d.QuantityInDuration(startTime, endTime));

        }
        /// <summary>
        /// 取定时间段内的特殊抗菌药物的DDD值.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>Decimal.</returns>
        internal Decimal TotalSpecialDddInDuration(DateTime startTime, DateTime endTime)
        {
            return this.IsSpecialAntibioticDrug ? this.DDD * this.DrugFees.Sum(d => d.QuantityInDuration(startTime, endTime)) : Decimal.Zero;
        }
        internal DepartmentDdd GetDepartmentDdd(DateTime startTime, DateTime endTime)
        {
            var result = new DepartmentDdd
            {
                DepartmentID = this.Origin_EXEC_DEPT,
                Ddd = this.TotalDddInDuration(startTime, endTime)
            };
            return result;
        }

        /// <summary>
        /// 获得特殊限制抗菌药物的科室及Ddd总消耗.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>DepartmentDdd.</returns>
        internal DepartmentDdd GetDepartmentSpecialDdd(DateTime startTime, DateTime endTime)
        {
            var result = new DepartmentDdd
            {
                DepartmentID = this.Origin_EXEC_DEPT,
                Ddd = this.TotalSpecialDddInDuration(startTime, endTime)
            };
            return result;
        }
        #endregion
        #region 药品种类
        /// <summary>
        /// 根据药品的种类获获得药物的编码和药物名称.
        /// </summary>
        /// <returns>EssentialDrugMessage.</returns>
        internal DrugMessage GetDrugCategoryMessage(DateTime startTime, DateTime endTime,EnumDrugCategory drugCategory)
        {
            var result = new DrugMessage
            {
                ProductID = this.Origin_CJID,
                ProductName = this.ProductName,
                Cost = this.DrugFees.Sum(d => d.ActualPriceInDuration(startTime, endTime))
            };

            switch (drugCategory)
            {
                case EnumDrugCategory.ALL_DRUG:

                    break;
                case EnumDrugCategory.ESSENTIAL_DRUG:
                    if (!IsEssential)
                    {
                        result = null;
                    }
                    break;
                case EnumDrugCategory.ANTIBIOTIC_DRUG:
                    if (!this.IsAntibioticDrug)
                    {
                        result = null;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion
        #region 基本药物

        internal DepartmentCost GetDepartmentEssentialDrugCost(DateTime startTime, DateTime endTime)
        {
            return this.IsEssential ? new DepartmentCost
            {
                DepartmentID = this.Origin_EXEC_DEPT,
                Cost = DrugCost(startTime, endTime),
                DrugCJID = this.Origin_CJID
            }
            : null;
        }
        #endregion
        #endregion

    }

}
