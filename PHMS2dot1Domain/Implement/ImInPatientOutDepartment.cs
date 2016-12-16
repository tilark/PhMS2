using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Models;
using ClassViewModelToDomain;

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
            //获取出院科室及人数
            var result = 
                (from b in
                    (from a in this.context.InPatients
                     where a.OutDate.HasValue && a.OutDate.Value >= startTime && a.OutDate.Value < endTime && !a.CaseNumber.Contains("XT")
                     group a by a.Origin_DEPT_ID into g
                     select new  { DepartmentID = (int)g.Key, RegisterPerson = g.Count() })
                              join c in this.context.Departments on b.DepartmentID equals c.Origin_DEPT_ID
                              select new InPatientOutDepartment { DepartmentID = b.DepartmentID, DepartmentName = c.DepartmentName, RegisterPerson = b.RegisterPerson }).ToList();
            return result;
        }

        public List<InPatientOutDepartmentDomain> GetInPatientOutDepartmentDetails(DateTime startTime, DateTime endTime)
        {
            var result =               
                  ( from a in this.context.InPatients
                    where a.OutDate.HasValue && a.OutDate.Value >= startTime && a.OutDate.Value < endTime && !a.CaseNumber.Contains("XT")   
                    join b in this.context.Departments on (int)a.Origin_DEPT_ID equals b.Origin_DEPT_ID             
                
                select new InPatientOutDepartmentDomain { CaseNumber = a.CaseNumber, Times = a.Times, DepartmentID = (int)a.Origin_DEPT_ID, DepartmentName = b.DepartmentName }).ToList();
            return result;
        }
    }
}
