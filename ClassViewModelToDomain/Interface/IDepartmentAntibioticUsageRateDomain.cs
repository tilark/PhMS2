using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain.Interface
{
    public interface IDepartmentAntibioticUsageRateDomain
    {
        List<DepartmentAntibioticUsageRateDomain> GetDepartmentAntibioticUsageRateDomain(DateTime startTime, DateTime endTime);
        Task<List<DepartmentAntibioticUsageRateDomain>> GetDepartmentAntibioticUsageRateDomainAsync(DateTime startTime, DateTime endTime);
    }
}
