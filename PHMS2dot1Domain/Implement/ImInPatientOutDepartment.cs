using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientOutDepartment : IInPatientOutDepartment
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatientOutDepartment(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InPatientOutDepartment> GetInPatientOutDepartment(DateTime startTime, DateTime endTime)
        {
            //获取出院人数
            var result = (from a in this.context.InPatients
                                               where a.OutDate.HasValue && a.OutDate.Value >= startTime && a.OutDate.Value < endTime && !a.CaseNumber.Contains("XT")
                                               group a by a.Origin_DEPT_ID into g
                                               select new InPatientOutDepartment { DepartmentID = (int)g.Key, RegisterPerson = g.Count() }).ToList();
            return result;
        }
    }
}
