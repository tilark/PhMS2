using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PhMS2dot1Domain.ViewModels;
using ClassViewModelToDomain;

namespace PhMS2dot1Domain.Models
{
    

    [Table("OutPatientDrugRecords")]
    public class OutPatientDrugRecord
    {
        #region Domain
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
        #endregion

        #region 扩展方法

        #region 抗菌药物
        /// <summary>
        /// Determines whether this instance contain antibiotic.
        /// </summary>
        /// <returns><c>true</c> if this instance is antibiotic; otherwise, <c>false</c>.</returns>
        /// 
        internal bool IsAntibiotic
        {
            get
            {
                return this.Origin_KSSDJ.HasValue && this.Origin_KSSDJ.Value >= 1 && this.Origin_KSSDJ.Value <= 3;
            }
        }
        internal string AntibioticCategoryNumber
        {
            get
            {
                return this.IsAntibiotic
                                ? this.ProductName
                                : String.Empty;
            }

        }

        internal int AntibioticCategoryID
        {
            get
            {
                return this.IsAntibiotic & this.Origin_CJID.HasValue ? this.Origin_CJID.Value : 0;
            }
        }
        internal Decimal AntibioticCost()
        {
            Decimal cost = 0;
            if (this.IsAntibiotic)
            {
                cost = this.ActualPrice;
            }
            return cost;
        }
        #endregion

        #region 药物价格
        /// <summary>
        /// 该处方的药品价格
        /// </summary>
        /// <returns>Decimal.</returns>
        internal Decimal DrugCost()
        {
            return ActualPrice;
        }
        #endregion

        #region 药物种类
        /// <summary>
        /// 根据药品的种类获获得药物的编码和药物名称.
        /// </summary>
        /// <returns>EssentialDrugMessage.</returns>
        internal DrugMessage GetDrugCategoryMessage(EnumDrugCategory drugCategory)
        {
            var result = new DrugMessage
            {

                ProductID = this.Origin_CJID.HasValue ? this.Origin_CJID.Value : -1,
                ProductName = this.ProductName,
                Cost = this.ActualPrice
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
                    if (! this.IsAntibiotic)
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
        /// <summary>
        /// 获得基本药物的编码和药物名称.
        /// </summary>
        /// <returns>EssentialDrugMessage.</returns>
        internal DrugMessage GetEssentialDrugMessage()
        {
            DrugMessage result = new DrugMessage();
            if (IsEssential)
            {
                result = new DrugMessage
                {
                    ProductID = this.Origin_CJID.HasValue ? this.Origin_CJID.Value : -1,
                    ProductName = this.ProductName,
                    Cost = ActualPrice
                };
            }

            return result;
        }
        #endregion
        #endregion

    }
}
