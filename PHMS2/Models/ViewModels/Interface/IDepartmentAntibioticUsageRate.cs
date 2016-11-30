using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2.Models.ViewModels.InPatientReporter;

namespace PHMS2.Models.ViewModels.Interface
{
    public interface IDepartmentAntibioticUsageRateList
    {
        DepartmentAntibioticUsageRate GetDepartmentAntibioticUsageRateList(DateTime startTime, DateTime endTime);
        Task<DepartmentAntibioticUsageRate> GetDepartmentAntibioticUsageRateListAsync(DateTime startTime, DateTime endTime);
    }
}
