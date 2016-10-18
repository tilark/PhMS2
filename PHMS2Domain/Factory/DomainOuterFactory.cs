using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2Domain.Interface;
using PHMS2Domain.Implement;
using PHMS2Domain.Models;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PHMS2Domain.Factory
{
    public class DomainOuterFactory : IDomainOuterFactory
    {
        #region Structer
        public DomainOuterFactory() 
        {

        }  
        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                   
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DomainInternalFactory() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion   

        public IRegisterPerson CreateRegisterPerson(EnumOutPatientCategories categories)
        {
            IRegisterPerson result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImRegisterPerson.GetOutPatientEmergencyRegisterPerson();
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImRegisterPerson.GetOutPatientRegisterPerson();
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImRegisterPerson.GetEmergencyRegisterPerson();
                    break;
            }
            return result;
        }
        public IAntibioticPerson CreateAntibioticPerson(EnumOutPatientCategories categories)
        {
            IAntibioticPerson result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImAntibioticPerson.GetOutPatientEmergencyAntibioticPerson();
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImAntibioticPerson.GetOutPatientAntibioticPerson();
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImAntibioticPerson.GetEmergencyAntibioticPerson();
                    break;
            }
            return result;
        }
        public IEssentialDrugCategoryNumbers CreateEssentialDrugCategoryNumbers()
        {
            var result = new ImGetEssentialDrugCategoryNumbers();
            return result;
        }

        public IDrugCategoriesNumbers CreateDrugCategoriesNumbers()
        {
            var result = new ImGetDrugCategoriesNumbers();
            return result;
        }

        public IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory)
        {
            IDrugTopRank drugTopRank = null;
            switch (drugCategory)
            {
                case EnumDrugCategory.ALL_DRUG:
                    drugTopRank = new ImDrugTopThirtyRank();
                    break;
                case EnumDrugCategory.ANTIBIOTIC_DRUG:
                    drugTopRank = new ImTopTenAntibioticRank();
                    break;
                case EnumDrugCategory.ANTIBIOTIC_DRUG_DEP:
                    drugTopRank = new ImTopTenAntibioticDepRank();
                    break;

            }
            return drugTopRank;
        }

        public IPatientCost CreatePatientCost(EnumOutPatientCategories categories)
        {
            IPatientCost result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImPatientCost.ImGetOutPatientEmergencyCost();
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    break;
                default:
                    break;
            }
            return result;
        }

        public IPrescriptionMessage CreatePrescriptionMessage(EnumOutPatientCategories categories)
        {
            IPrescriptionMessage result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImPrescriptionMessage.ImOutPatientEmergencyPrescriptionMessage();
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImPrescriptionMessage.ImOutPatientPrescriptionMessage();
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImPrescriptionMessage.ImEmergencyPrescriptionMessage();
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
