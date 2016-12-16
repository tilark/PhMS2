using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain.Interface
{
    public interface IInPatientOutDepartmentDetail
    {
        List<InPatientOutDepartmentDomain> GetInPatientOutDepartmentDetail(DateTime startTime, DateTime endTime);
    }
}
