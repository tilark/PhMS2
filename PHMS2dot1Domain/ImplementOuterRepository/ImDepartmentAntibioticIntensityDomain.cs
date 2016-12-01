using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;
using PhMS2dot1Domain.ViewModels;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    /// <summary>
    /// 获取科室抗菌药物强度.
    /// </summary>
    public class ImDepartmentAntibioticIntensityDomain : IDepartmentAntibioticIntensityDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        private readonly IInnerRepository innerRepository;
        public ImDepartmentAntibioticIntensityDomain(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
            this.innerRepository = new InnerRepository(this.innerFactory);
        }
        public List<DepartmentAntibioticIntensityDomain> GetDepartmentAntibioticIntensityDomain(DateTime startTime, DateTime endTime)
        {
            var result = new List<DepartmentAntibioticIntensityDomain>();
            try
            {
                //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);

                //住院抗菌药物消耗量（累计DDD数）
                var departmentDddList = inPatientFromDrugRecordList.SelectMany(i => i.GetDepartmentDddList(startTime, endTime)).GroupBy(a => a.DepartmentID).Select(g => new DepartmentDdd { DepartmentID = g.Key, Ddd = g.Sum(b => b.Ddd) }).ToList();
                //同期收治患者人天数=同期出院患者人数×同期出院患者平均住院天数
                var inPatientList = this.innerFactory.CreateInPatientInDuration().GetInPatientInDruation(startTime, endTime);
                var inPatientNumberDayList = inPatientList.GroupBy(i => i.Origin_DEPT_ID).Select(g => new DepartmentInPatientDay { DepartmentID = (int)g.Key, InPatientDay = g.Count() * g.Sum(b => b.InHospitalDays) }).ToList();
                //获取科室集合
                var departments = this.innerFactory.CreateDepartment().GetDepartment();
                //join
                var inPatientNumberDayJoinDepartmentList = inPatientNumberDayList.Join(departments, numberDay => numberDay.DepartmentID, department => department.DepartmentID, (numberDay, department) => new { DepartmentID = numberDay.DepartmentID, DepartmentName = department.DepartmentName, InPatientDay = numberDay.InPatientDay }).ToList();

                result = departmentDddList.Join(inPatientNumberDayJoinDepartmentList, departmentDdd => departmentDdd.DepartmentID, inPatientNumberDay => inPatientNumberDay.DepartmentID, (departmentDdd, inPatientNumberDay) => new DepartmentAntibioticIntensityDomain { DepartmentID = departmentDdd.DepartmentID, DepartmentName = inPatientNumberDay.DepartmentName, AntibioticDdd = departmentDdd.Ddd, PersonNumberDays = inPatientNumberDay.InPatientDay }).ToList();



            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("读取数据失败！{0}", e.Message));
            }
            

            return result;
        }
    }
}
