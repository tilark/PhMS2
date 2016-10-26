using PHMS2.Models.ViewModels.InPatientReporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2.Models.ViewModels.Interface
{
    public interface ISpecialAntibioticUsageRate
    {
        SpecialAntibioticUsageRate GetSpecialAntibioticUsageRate(DateTime startTime, DateTime endTime);
    }
}
