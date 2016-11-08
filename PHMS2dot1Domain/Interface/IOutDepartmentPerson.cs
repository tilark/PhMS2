using PhMS2dot1Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    public interface IOutDepartmentPerson
    {
        List<OutDepartmentPerson> GetOutDepartmentPerson(DateTime startTime, DateTime endTime);

        Task<List<OutDepartmentPerson>> GetOutDepartmentPersonAsync(DateTime startTime, DateTime endTime);
    }
}
