using PHMS2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2.Models.ViewModels.Interface
{
    public interface IDepartmentAntibioticUsageRateList
    {
        DepartmentAntibioticUsageRate GetDepartmentAntibioticUsageRateList(DateTime startTime, DateTime endTime);
    }
}
