using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.ViewModels.Implement;
using PhMS2dot1Domain.Factories;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.Factories
{
    public class InPatientReporterFactory : IInPatientReporterFactory
    {
        private readonly IDomainFacotry factory;

        public InPatientReporterFactory(IDomainFacotry factory)
        {
            this.factory = factory;
        }

        public IDepartmentAntibioticIntensity CreateDepartmentAntibioticIntensity()
        {
            return new ImDepartmentAntibioticIntensity(this.factory);
        }

        public IDepartmentAntibioticUsageRateList CreateDepartmentAntibioticUsageRateList()
        {
            return new ImDepartmentAntibioticUsageRateList(this.factory);
        }

        public IDepartmentEssentialUsageRate CreateDepartmentEssentialUsageRate()
        {
            return new ImDepartmentEssentialUsageRate(this.factory);
        }

        public IDrugTopThirtyDescription CreateDrugTopThirtyDescription()
        {
           return new ImDrugTopThirtyDescription(this.factory);
        }

        public IInPatientAntibioticUsageRate CreateInPatientAntibioticUsageRate()
        {
            return new ImInPatientAntibioticUsageRate(this.factory);
        }

        public IInPatientAverageAntibioticCategoryRate CreateInPatientAverageAntibioticCategoryRate()
        {
            return new ImInPatientAverageAntibioticCategoryRate(this.factory);
        }

        public IInPatientAverageAntibioticCostRate CreateInPatientAverageAntibioticCostRate()
        {
            return new ImInPatientAverageAntibioticCostRate(this.factory);
        }

        public IInPatientDrugMessage CreateInPatientDrugMessage()
        {
            return new ImInPatientDrugMessage(this.factory);
        }

        public ISpecialAntibioticUsageRate CreateSpecialAntibioticUsageRate()
        {
            return new ImSpecialAntibioticUsageRate(this.factory);
        }
    }
}