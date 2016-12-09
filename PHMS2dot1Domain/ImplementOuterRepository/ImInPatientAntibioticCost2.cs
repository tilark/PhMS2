using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{

    public class ImInPatientAntibioticCost2 : IInPatientAntibioticCost
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientAntibioticCost2(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }

        public decimal GetInPatientAntibioticCost(DateTime startTime, DateTime endTime)
        {
            var result = 0M;
            result = this.innerFactory.CreateInPatientAntibioticCost().GetInPatientAntibioticCost(startTime, endTime);
            return result;
        }
    }
}
