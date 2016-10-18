using PHMS2Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;

namespace PHMS2.Models.ViewModel.Interface
{
    public interface IAntibioticUsageRate
    {
        AntibioticUsageRate GetAntibioticUsageRate(DateTime startTime, DateTime endTime, EnumOutPatientCategories categories);

    }
}
