using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    public interface IInPatientAntibioticCostDomain
    {
        Decimal GetInPatientAntibioticCost(DateTime startTime, DateTime endTime);
    }
}
