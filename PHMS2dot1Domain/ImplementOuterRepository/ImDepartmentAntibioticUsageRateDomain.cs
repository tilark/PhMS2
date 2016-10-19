using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;
using ClassViewModelToDomain.Interface;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImDepartmentAntibioticUsageRateDomain : IDepartmentAntibioticUsageRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        private readonly IInnerRepository innerRepository;
        public ImDepartmentAntibioticUsageRateDomain(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
            this.innerRepository = new InnerRepository(this.innerFactory);
        }
        public List<DepartmentAntibioticUsageRateDomain> GetDepartmentAntibioticUsageRateDomain(DateTime startTime, DateTime endTime)
        {
            var result = new List<DepartmentAntibioticUsageRateDomain>();
            try
            {
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);
                var antibioticDepartmentPerson = inPatientFromDrugRecordList.SelectMany(i => i.AntibioticDepartmentPersonList(startTime, endTime)).ToList().GroupBy(a => a.DepartmentID).Select(g => new DepartmentAntibioticUsageRateDomain { DepartmentID = g.Key, AntibioticPerson = g.Sum(a => a.PersonNumber) }).ToList();



                //var inPatientList = this.innerFactory.CreateInPatientInDuration().GetInPatientInDruation(startTime, endTime);

                //var departmentPersonList = inPatientList.GroupBy(a => a.Origin_DEPT_ID).Select(g => new DepartmentAntibioticUsageRateDomain { DepartmentID = (int)g.Key, RegisterPerson = g.Count() }).ToList();

                var departmentPersonList = this.innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPerson(startTime, endTime);
                var departments = this.innerFactory.CreateDepartment().GetDepartment();

                var result1 = antibioticDepartmentPerson.Join(departmentPersonList, antibioticPerson => antibioticPerson.DepartmentID, departmentPerson => departmentPerson.DepartmentID, (antibioticPerson, departmentPerson) => new DepartmentAntibioticUsageRateDomain { DepartmentID = antibioticPerson.DepartmentID, AntibioticPerson = antibioticPerson.AntibioticPerson, RegisterPerson = departmentPerson.InPatientNumber }).ToList();

                result = result1.Join(departments, result11 => result11.DepartmentID, department => department.Origin_DEPT_ID, (result11, department) => new DepartmentAntibioticUsageRateDomain { DepartmentID = result11.DepartmentID, DepartmentName = department.DepartmentName, AntibioticPerson = result11.AntibioticPerson, RegisterPerson = result11.RegisterPerson }).ToList();
            }
            catch (Exception e)
            {

                throw new ArgumentNullException("GetDepartmentAntibioticUsageRateDomain get null!", e);
            }
            
            return result;
        }
    }
}
