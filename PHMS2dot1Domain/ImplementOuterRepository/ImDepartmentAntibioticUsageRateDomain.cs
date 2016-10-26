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
    /// <summary>
    /// ImDepartmentAntibioticUsageRateDomain.获取各科室抗菌药物使用率集合
    /// </summary>
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
                //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);

                //获取各科室抗菌药物使用人数
                var antibioticDepartmentPerson = inPatientFromDrugRecordList.SelectMany(i => i.AntibioticDepartmentPersonList(startTime, endTime)).ToList().GroupBy(a => a.DepartmentID).Select(g => new DepartmentAntibioticUsageRateDomain { DepartmentID = g.Key, AntibioticPerson = g.Sum(a => a.PersonNumber) }).ToList();

                //获取同期出院人数
                var departmentPersonList = this.innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPerson(startTime, endTime);
                //获取科室集合
                var departments = this.innerFactory.CreateDepartment().GetDepartment();

                //join
                var result1 = antibioticDepartmentPerson.Join(departmentPersonList, antibioticPerson => antibioticPerson.DepartmentID, departmentPerson => departmentPerson.DepartmentID, (antibioticPerson, departmentPerson) => new DepartmentAntibioticUsageRateDomain { DepartmentID = antibioticPerson.DepartmentID, AntibioticPerson = antibioticPerson.AntibioticPerson, RegisterPerson = departmentPerson.InPatientNumber }).ToList();

                result = result1.Join(departments, result11 => result11.DepartmentID, department => department.Origin_DEPT_ID, (result11, department) => new DepartmentAntibioticUsageRateDomain { DepartmentID = result11.DepartmentID, DepartmentName = department.DepartmentName, AntibioticPerson = result11.AntibioticPerson, RegisterPerson = result11.RegisterPerson }).ToList();
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据处理出错!{0}", e.Message));
            }
            
            return result;
        }
    }
}
