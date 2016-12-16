using ClassViewModelToDomain;
using PhMS2dot1Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    public interface IInPatientOutDepartment
    {
        /// <summary>
        /// 出院患者人数.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;InPatientOutDepartment&gt;.</returns>
        List<InPatientOutDepartment> GetInPatientOutDepartment(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获取出院病人的详细信息，包括病人的住院号及住院次数
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;InPatientOutDepartment&gt;.</returns>
        List<InPatientOutDepartmentDomain> GetInPatientOutDepartmentDetails(DateTime startTime, DateTime endTime);
    }
}
