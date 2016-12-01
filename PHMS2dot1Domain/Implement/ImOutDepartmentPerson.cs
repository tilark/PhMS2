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
            int tryTimes = 0;
            do
            {

                tryTimes++;
                try
                {
                    result = this.innerFactory.CreateInPatientInDuration().GetInPatientInDruation(startTime, endTime).GroupBy(a => a.Origin_DEPT_ID).Select(g => new OutDepartmentPerson { DepartmentID = (int)g.Key, InPatientNumber = g.Count() }).ToList();
                    break;
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException)
                {

                }
                catch (Exception)
                {

                    throw;
                }
                
            } while (tryTimes < 51);
           

            return result;
        }

        public async Task<List<OutDepartmentPerson>> GetOutDepartmentPersonAsync(DateTime startTime, DateTime endTime)
        {
            var result = new List<OutDepartmentPerson>();
            try
            {
                var list = await this.innerFactory.CreateInPatientInDuration().GetInPatientInDruationAsync(startTime, endTime);
                result = list.GroupBy(a => a.Origin_DEPT_ID).Select(g => new OutDepartmentPerson { DepartmentID = (int)g.Key, InPatientNumber = g.Count() }).ToList();
               
            }
            catch (Exception)
            {

                //result = null;
            }

            return result;
        }
    }
}
