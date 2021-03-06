﻿using System;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.ViewModels.Implement;
using ClassViewModelToDomain;
using ClassViewModelToDomain.IFactory;
using ClassViewModelToDomain.Interface;
using PHMS2Domain.Factory;

namespace PHMS2.Models.Factories
{
    public class ReporterViewFactory : IReporterViewFactory 
    {
        private readonly IDomainFacotry DomainFactory;

        public ReporterViewFactory(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public virtual IEssentialDrugRate CreateEssentialDrugRate()
        {
            IEssentialDrugRate drugRateModel = null;
            drugRateModel = new ImEssentialDrugCategory(this.DomainFactory);
            return drugRateModel;
        }
        public virtual IAntibioticUsageRate CreateAntibioticUsageRate()
        {
            IAntibioticUsageRate result = null;
            result = new ImAntibioticUsageRate(this.DomainFactory);
            return result;
        }
        
        public virtual IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory)
        {
            IDrugTopRank drugTopRank = null;
            switch (drugCategory)
            {
                case EnumDrugCategory.ALL_DRUG:
                    drugTopRank = new ImDrugTopThirtyRank(this.DomainFactory);
                    break;
                case EnumDrugCategory.ANTIBIOTIC_DRUG:
                    drugTopRank = new ImTopTenAntibioticRank(this.DomainFactory);
                    break;
                case EnumDrugCategory.ANTIBIOTIC_DRUG_DEP:
                    drugTopRank = new ImTopTenAntibioticDepRank(this.DomainFactory);
                    break;

            }
            return drugTopRank;
        }


        public virtual IPrescriptionMessageCollection CreatePrescriptionMessageCollection()
        {
            IPrescriptionMessageCollection result = null;
            result = new ImPrescriptionMessageCollection(this.DomainFactory);
            return result;
        }

        public virtual IPatientAverageCost CreatePatientAverageCost(EnumOutPatientCategories categories)
        {

            IPatientAverageCost result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImPatientAverageCost.ImOutPatientAverageCost(this.DomainFactory);

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

        public IDrugCategoryRate CreateDrugCategoryRate()
        {
            IDrugCategoryRate result = null;
            result = new ImDrugCategoryRate(this.DomainFactory);
            return result;
        }

        public IDrugTopThirtyDescription CreateDrugTopThirtyDescription()
        {
            return new ImDrugTopThirtyDescription(this.DomainFactory);
        }
    }
}