using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Models;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.Implement
{
    /// <summary>
    /// ImOutDepartmentPerson.获得取定时间段内的出院病人的住院科室及科室出院人数
    /// </summary>
    public class ImOutDepartmentPerson : IOutDepartmentPerson
    {
        private readonly IDomain2dot1InnerFactory innerFactory;

        public ImOutDepartmentPerson(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public List<OutDepartmentPerson> GetOutDepartmentPerson(DateTime startTime, DateTime endTime)
        {
            var result = new List<OutDepartmentPerson>();
            try
            {
                var inPatientList = this.innerFactory.CreateInPatientInDuration().GetInPatientInDruation(startTime, endTime);

                result = inPatientList.GroupBy(a => a.Origin_DEPT_ID).Select(g => new OutDepartmentPerson { DepartmentID = (int)g.Key, InPatientNumber = g.Count() }).ToList();
            }
            catch (Exception)
            {

                result = null;
            }

            return result;
        }
    }
}
