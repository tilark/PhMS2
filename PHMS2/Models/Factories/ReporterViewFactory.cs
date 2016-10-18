using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;
using PHMS2.Models.ViewModel.Implement;
using PHMS2.Models.ViewModel.Interface;
using PHMS2Domain.Factory;
using System;

namespace PHMS2.Models.Factories
{
    public class ReporterViewFactory : IReporterViewFactory 
    {
        
        public virtual IEssentialDrugRate CreateEssentialDrugRate()
        {
            IEssentialDrugRate drugRateModel = null;
            drugRateModel = new ImEssentialDrugCategory();
            return drugRateModel;
        }
        public virtual IAntibioticUsageRate CreateAntibioticUsageRate()
        {
            IAntibioticUsageRate result = null;
            result = new ImAntibioticUsageRate();
            return result;
        }
        
        public virtual IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory)
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


        public virtual IPrescriptionMessageCollection CreatePrescriptionMessageCollection()
        {
            IPrescriptionMessageCollection result = null;
            result = new ImPrescriptionMessageCollection();
            return result;
        }

        public virtual IPatientAverageCost CreatePatientAverageCost(EnumOutPatientCategories categories)
        {

            IPatientAverageCost result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImPatientAverageCost.ImOutPatientAverageCost();

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

       
    }
}