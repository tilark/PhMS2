using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain.Interface
{
    public interface IDepartmentEssentialUsageRateDomain
    {
        List<DepartmentEssentialUsageRateDomain> GetDepartmentEssentialUsageRateDomain(DateTime startTime, DateTime endTime);
    }
}
