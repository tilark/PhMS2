using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.ImplementOuterRepository;
using ClassViewModelToDomain.IFactory;

namespace PhMS2dot1Domain.Factories
{
    public class Domain2dot1OuterFactory : IDomainFacotry
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public Domain2dot1OuterFactory(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        #region 门诊信息


        public IAntibioticPerson CreateAntibioticPerson(EnumOutPatientCategories categories)
        {
            IAntibioticPerson result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImAntibioticPerson.GetOutPatientEmergencyAntibioticPerson(this.innerFactory);
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImAntibioticPerson.GetOutPatientAntibioticPerson(this.innerFactory);
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImAntibioticPerson.GetEmergencyAntibioticPerson(this.innerFactory);
                    break;
            }
            return result;
        }
        public IDrugCategoriesNumbers CreateDrugCategoriesNumbers()
        {
            var result = new ImGetDrugCategoriesNumbers(this.innerFactory);
            return result;
        }

        public IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory)
        {
            IDrugTopRank drugTopRank = null;
            switch (drugCategory)
            {
                case EnumDrugCategory.ALL_DRUG:
                    drugTopRank = new ImDrugTopThirtyRank(this.innerFactory);
                    break;
                case EnumDrugCategory.ANTIBIOTIC_DRUG:
                    drugTopRank = new ImTopTenAntibioticRank(this.innerFactory);
                    break;
                case EnumDrugCategory.ANTIBIOTIC_DRUG_DEP:
                    drugTopRank = new ImTopTenAntibioticDepRank(this.innerFactory);
                    break;

            }
            return drugTopRank;
        }

        public IEssentialDrugCategoryNumbers CreateEssentialDrugCategoryNumbers()
        {
            var result = new ImGetEssentialDrugCategoryNumbers(this.innerFactory);
            return result;
        }

        public IPatientCost CreatePatientCost(EnumOutPatientCategories categories)
        {
            IPatientCost result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImPatientCost.ImGetOutPatientEmergencyCost(this.innerFactory);
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
                    result = new ImPrescriptionMessage.ImOutPatientEmergencyPrescriptionMessage(this.innerFactory);
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImPrescriptionMessage.ImOutPatientPrescriptionMessage(this.innerFactory);
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImPrescriptionMessage.ImEmergencyPrescriptionMessage(this.innerFactory);
                    break;
                default:
                    break;
            }
            return result;
        }

        public IRegisterPerson CreateRegisterPerson(EnumOutPatientCategories categories)
        {
            IRegisterPerson result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImRegisterPerson.GetOutPatientEmergencyRegisterPerson(this.innerFactory);
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImRegisterPerson.GetOutPatientRegisterPerson(this.innerFactory);
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImRegisterPerson.GetEmergencyRegisterPerson(this.innerFactory);
                    break;
            }
            return result;
        }
        #endregion
        #region 住院信息
        public IDepartmentAntibioticIntensityDomain CreateDepartmentAntibioticIntensityDomain()
        {
            return new ImDepartmentAntibioticIntensityDomain(this.innerFactory);
        }

        public IDepartmentAntibioticUsageRateDomain CreateDepartmentAntibioticUsageRateDomain()
        {
            return new ImDepartmentAntibioticUsageRateDomain(this.innerFactory);
        }

        public ISpecialAntibioticDdds CreateSpecialAntibioticDdds()
        {
            return new ImSpecialAntibioticDdds(this.innerFactory);
        }

        public ITotalAntibioticDdds CreateTotalAntibioticDdds()
        {
            return new ImTotalAntibioticDdds(this.innerFactory);
        }

        public IDepartmentEssentialUsageRateDomain CreateDepartmentEssentialUsageRateDomain()
        {
            return new ImDepartmentEssentialUsageRateDomain(this.innerFactory);
        }

        public IInPatientAntibioticCost CreateInPatientAntibioticCost()
        {
            return new ImInPatientAntibioticCost(this.innerFactory);
        }

        public IPatientCost CreateInPatientDrugCost()
        {
            return new ImInPatientDrugCost(this.innerFactory);
        }

        public IAntibioticCategoryNumber CreateInPatientAntibioticCategoryNumber()
        {
            return new ImInPatientAntibioticCategoryNumber(this.innerFactory);
        }

        /// <summary>
        /// 获取同期住院抗菌药物使用人数
        /// </summary>
        /// <returns>IAntibioticPerson.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IAntibioticPerson CreateInPatientAntibioticPerson()
        {
            return new ImInPatientAntibioticPerson(this.innerFactory);
        }
        #endregion


    }
}
