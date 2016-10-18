using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.ViewModels.Implement;

namespace PHMS2.Models.Factories
{
    public class InPatientReporterFactory : IInPatientReporterFactory
    {
        public IDepartmentAntibioticUsageRateList CreateDepartmentAntibioticUsageRateList()
        {
            return new ImDepartmentAntibioticUsageRateList();
        }
    }
}