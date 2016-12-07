using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImInPatientAntibioticUsageRate : IInPatientAntibioticUsageRate
    {
        private readonly IDomainFacotry factory;

        public ImInPatientAntibioticUsageRate(IDomainFacotry factory)
        {
            this.factory = factory;
        }
        public InPatientAntibioticCostRate GetInPatientAntibioticCostRate(DateTime startTime, DateTime endTime)
        {
            var result = new InPatientAntibioticCostRate();
            try
            {
                var temp = this.factory.CreateInPatientAntibioticCostRateDomain().GetInPatientAntibioticCostRateDomain(startTime, endTime);
                result = new InPatientAntibioticCostRate
                {
                    TotalAntibioticCost =temp.TotalAntibioticCost,
                    TotalDrugCost = temp.TotalDrugCost
                };
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}