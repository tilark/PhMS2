using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientAntibioticCostRateDomain2 : IInPatientAntibioticCostRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;

        public ImInPatientAntibioticCostRateDomain2(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;

        }

        public InPatientAntibioticCostRateDomain GetInPatientAntibioticCostRateDomain(DateTime startTime, DateTime endTime)
        {
            var result = new InPatientAntibioticCostRateDomain();
            try
            {
                result.TotalDrugCost = this.innerFactory.CreateInPatientAllDrugCost().GetInPatientAllDrugCost(startTime, endTime);
                result.TotalAntibioticCost = this.innerFactory.CreateInPatientAntibioticCost().GetInPatientAntibioticCost(startTime, endTime);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
