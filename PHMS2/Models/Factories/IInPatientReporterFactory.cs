using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2.Models.Factories
{
    public interface IInPatientReporterFactory
    {
        IDepartmentAntibioticUsageRateList CreateDepartmentAntibioticUsageRateList();
    }
}
