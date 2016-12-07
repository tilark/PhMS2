using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain.Interface
{
    public interface IInPatientAntibioticCostRateDomain
    {
        InPatientAntibioticCostRateDomain GetInPatientAntibioticCostRateDomain(DateTime startTime, DateTime endTime);
    }
}
