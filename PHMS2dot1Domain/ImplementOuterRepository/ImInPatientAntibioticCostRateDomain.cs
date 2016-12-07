using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientAntibioticCostRateDomain : IInPatientAntibioticCostRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;

        public ImInPatientAntibioticCostRateDomain(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;

        }
        public InPatientAntibioticCostRateDomain GetInPatientAntibioticCostRateDomain(DateTime startTime, DateTime endTime)
        {
            var result = new InPatientAntibioticCostRateDomain();
            try
            {
                var inPatientAllDrugCostList = this.innerFactory.CreateInPatientAllDrugRecordFee().GetInpatientDrugRecordFees(startTime, endTime);
                result.TotalDrugCost = inPatientAllDrugCostList.Sum(a => a.ActualPrice);
                result.TotalAntibioticCost = inPatientAllDrugCostList.Where(a => a.IsAntibiotic).Sum(b => b.ActualPrice);
            }
            catch (Exception)
            {
                
                throw;
            }
            return result;
        }
    }
}
