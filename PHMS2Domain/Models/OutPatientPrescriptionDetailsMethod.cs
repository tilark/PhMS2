using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2Domain.Models
{
    public partial class OutPatientPrescriptionDetails
    {
        #region 抗菌药
        /// <summary>
        /// Determines whether this instance contain antibiotic.
        /// </summary>
        /// <returns><c>true</c> if this instance is antibiotic; otherwise, <c>false</c>.</returns>
        /// 
        internal bool IsAntibiotic
        {
            get
            {
                return DrugMaintenances.AntibioticManageLevelId.HasValue
                && DrugMaintenances.AntibioticManageLevelId.Value > 0;
            }
        }
        internal bool IsContainAntibiotic()
        {

            return DrugMaintenances.AntibioticManageLevelId.HasValue
                && DrugMaintenances.AntibioticManageLevelId.Value >=1
                && DrugMaintenances.AntibioticManageLevelId.Value <= 3;
        }

        internal Decimal AntibioticCost()
        {
            Decimal cost = 0;
            if (IsContainAntibiotic())
            {
                cost = DrugMaintenances.UnitCost * Quantity;
            }
            return cost;
        }
        internal string AntibioticCategoryNumber
        {
            get
            {
                return this.IsAntibiotic
                                ? DrugMaintenances.ProductNumber
                                : String.Empty;
            }

        }
        #endregion

        #region 药品类型

        internal int EssentialCount
        {
            get
            {
                return DrugMaintenances.IsEssential ? 1 : 0;
            }
        }
        internal bool IsEssential
        {
            get
            {
                return DrugMaintenances.IsEssential;
            }
        }
        internal string DrugNumber
        {
            get
            {
                return DrugMaintenances.ProductNumber;
            }
        }
        internal string EssentialDrugNumber
        {
            get
            {
                return IsEssential ? DrugMaintenances.ProductNumber : String.Empty;
            }
        }
        internal Decimal GetProductNumberCost(string productorNumber)
        {

            return IsContainProductNumber(productorNumber) ? DrugCost() : Decimal.Zero;
        }
        internal bool IsContainProductNumber(string productorNumber)
        {
            return String.Compare(DrugMaintenances.ProductNumber, productorNumber) == 0;
        }
        #endregion
        #region 药物种类
        /// <summary>
        /// 根据药品的种类获获得药物的编码和药物名称.
        /// </summary>
        /// <returns>EssentialDrugMessage.</returns>
        internal DrugMessage GetDrugCategoryMessage(EnumDrugCategories drugCategory)
        {
            var result = new DrugMessage
            {
                ProductNumber = DrugMaintenances.ProductNumber,
                ProductName = DrugMaintenances.ProductName,
                Cost = DrugMaintenances.UnitCost * Quantity
            }; ;

            switch (drugCategory)
            {
                case EnumDrugCategories.ALL_DRUG:
                    
                    break;
                case EnumDrugCategories.ESSENTIAL_DRUG:
                    if (!IsEssential)
                    {
                        result = null;
                    }
                    break;
                case EnumDrugCategories.ANTIBIOTIC_DRUG:
                    if (! (DrugMaintenances.AntibioticManageLevelId >= 1 && DrugMaintenances.AntibioticManageLevelId <= 3))
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
        internal EssentialDrugMessage GetEssentialDrugMessage()
        {
            EssentialDrugMessage result = new EssentialDrugMessage();
            if (IsEssential)
            {
                result = new EssentialDrugMessage
                {
                    ProductNumber = DrugMaintenances.ProductNumber,
                    ProductName = DrugMaintenances.ProductName,
                    Cost = DrugMaintenances.UnitCost * Quantity
                };
            }

            return result;
        }
        #endregion
        #region 全部药物        
        /// <summary>
        /// 该处方的药品价格
        /// </summary>
        /// <returns>Decimal.</returns>
        internal Decimal DrugCost()
        {
            return DrugMaintenances.UnitCost * Quantity;
        }
        /// <summary>
        /// 获得基本药物的编码和药物名称.
        /// </summary>
        /// <returns>EssentialDrugMessage.</returns>
        internal AllDrugMessage GetAllDrugMessage()
        {
            AllDrugMessage result = null;

            result = new AllDrugMessage
            {
                ProductNumber = DrugMaintenances.ProductNumber,
                ProductName = DrugMaintenances.ProductName,
                Cost = DrugMaintenances.UnitCost * Quantity
            };
            return result;
        }
        #endregion

    }
}