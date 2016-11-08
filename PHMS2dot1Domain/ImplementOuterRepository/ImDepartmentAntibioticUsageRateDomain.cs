using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Models;

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


                //获取同期出院人数
                //将时间分为几个片段再取数，整合后再存到result中。
                var duration = endTime.Subtract(startTime).Days;
                var durationDay = 3;
                int maxDays = (int)duration / durationDay + 1;
                //var startTime2 = startTime;
                //var endTime2 = startTime2.AddDays(durationDay);
                var departmentPersonTotoalList = new List<OutDepartmentPerson>();
                var inPatientFromDrugRecordTotalList = new List<InPatient>();

                Parallel.For(0, maxDays, (i, state) =>
                {
                    //do
                    //{
                    var startTime2 = startTime.AddDays( i * durationDay);
                    var endTime2 = startTime2.AddDays( durationDay);
                   
                    if (endTime2 > endTime)
                    {
                        endTime2 = endTime;
                    }
                    PhMS2dot1DomainContext context = new PhMS2dot1DomainContext();
                    IInnerRepository innerRepository = new InnerRepository(new Domain2dot1InnerFactory(context));
                    //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                    //var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime2, endTime2);
                    //inPatientFromDrugRecordTotalList.AddRange(inPatientFromDrugRecordList);

                    //获取各科室抗菌药物使用人数
                    //var antibioticDepartmentPerson = inPatientFromDrugRecordList.SelectMany(i => i.AntibioticDepartmentPersonList(startTime, endTime)).ToList().GroupBy(a => a.DepartmentID).Select(g => new DepartmentAntibioticUsageRateDomain { DepartmentID = g.Key, AntibioticPerson = g.Sum(a => a.PersonNumber) }).ToList();

                    var departmentPersonList = innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPerson(startTime2, endTime2);
                    //var departmentPersonList = await this.innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPersonAsync(startTime2, endTime2);
                    departmentPersonTotoalList.AddRange(departmentPersonList);



                    //var result1 = antibioticDepartmentPerson.Join(departmentPersonList, antibioticPerson => antibioticPerson.DepartmentID, departmentPerson => departmentPerson.DepartmentID, (antibioticPerson, departmentPerson) => new DepartmentAntibioticUsageRateDomain { DepartmentID = antibioticPerson.DepartmentID, AntibioticPerson = antibioticPerson.AntibioticPerson, RegisterPerson = departmentPerson.InPatientNumber }).ToList();

                    //result = result1.Join(departments, result11 => result11.DepartmentID, department => department.Origin_DEPT_ID, (result11, department) => new DepartmentAntibioticUsageRateDomain { DepartmentID = result11.DepartmentID, DepartmentName = department.DepartmentName, AntibioticPerson = result11.AntibioticPerson, RegisterPerson = result11.RegisterPerson }).ToList();

                    //result = result2.Join(antibioticDepartmentPerson, departmentPerson => departmentPerson.DepartmentID, antibioticPerson => antibioticPerson.DepartmentID, (departmentPerson, antibioticPerson) => new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = departmentPerson.DepartmentName, RegisterPerson = departmentPerson.RegisterPerson, AntibioticPerson = antibioticPerson.AntibioticPerson }).ToList();

                    //startTime2 = endTime2;
                    //endTime2 = startTime2.AddDays(durationDay);
                    //}
                    //while (startTime2 < endTime);

                });

                //获取科室集合

                var departments = this.innerFactory.CreateDepartment().GetDepartment();

                //需再group
                var departmentPersonResult = departmentPersonTotoalList.GroupBy(a => a.DepartmentID).Select(g => new { DepartmentID = g.Key, InPatientNumber = g.Sum(b => b.InPatientNumber) });
                //join
                result = departmentPersonResult.Join(departments, departmentPerson => departmentPerson.DepartmentID, department => department.Origin_DEPT_ID, (departmentPerson, department) => new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = department.DepartmentName, RegisterPerson = departmentPerson.InPatientNumber }).ToList();
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据处理出错!{0}", e.Message));
            }

            return result;
        }
    }
}
