using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;
using PhMS2dot1Domain.ViewModels;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImDepartmentAntibioticUsageRateDomain2 : IDepartmentAntibioticUsageRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        private readonly IInnerRepository innerRepository;

        public ImDepartmentAntibioticUsageRateDomain2(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
            this.innerRepository = new InnerRepository(this.innerFactory);

        }

        public List<DepartmentAntibioticUsageRateDomain> GetDepartmentAntibioticUsageRateDomain(DateTime startTime, DateTime endTime)
        {
            var result = new List<DepartmentAntibioticUsageRateDomain>();
            try
            {               
                var inpatientAnbtioticDrugRecordFees = this.innerFactory.CreateInPatientAntibioticDrugRecordFee().GetInpatientDrugRecordFees(startTime, endTime);
                //antibioticPersonList 集合包括取定时间段内的科室与抗菌药物有效人数和在开始时间段前的抗菌药物有效人数
                var antibioticPersonList = new List<InPatientDepartmentCost>();
                //分成两部分，一部分出院病人为在取定时间段内
                var inDurationList = (from a in inpatientAnbtioticDrugRecordFees
                                      where a.OutDate.Value >= startTime
                                      group a by new { a.InPatientID, a.DepartmentID } into b
                                      where b.Sum(c => c.ActualPrice) > 0
                                      select new InPatientDepartmentCost { InPatientID = b.Key.InPatientID, DepartmentID = (int)b.Key.DepartmentID, Cost = b.Sum(c => c.ActualPrice), Count = 1 }).ToList();

                
                var inDurationAntibioticPerson = from a in inDurationList
                                                 group a by a.DepartmentID into g
                                                 select new InPatientDepartmentCost { DepartmentID = g.Key, Count = g.Count() };
                antibioticPersonList.AddRange(inDurationAntibioticPerson);
                //另一部分为出院病人在startTime之前已出院，但是在取定时间段内还有费用产生，如退费等
                var preStartTimeList = inpatientAnbtioticDrugRecordFees.Where(a => a.OutDate.Value < startTime).AsParallel().ToList();

                if (preStartTimeList.Count > 0)
                {

                    //获取住院时间与startTime之间的所有费用，即<startTime的总费用
                    var preStartTimeCostList = from a in preStartTimeList
                                               where a.InDate < startTime
                                               group a by new { a.InPatientID, a.DepartmentID } into b
                                               select new InPatientDepartmentCost { InPatientID = b.Key.InPatientID, DepartmentID = (int)b.Key.DepartmentID, Cost = b.Sum(c => c.ActualPrice), Count = 0 };
                    //获取住院时间与endTime之间的所有费用，即<endTime的总费用，用来判断是加1还是减1，还是为0
                    var preEndTimeCostList = from a in preStartTimeList
                                             where a.InDate < endTime
                                             group a by new { a.InPatientID, a.DepartmentID } into b
                                             select new InPatientDepartmentCost { InPatientID = b.Key.InPatientID, DepartmentID = (int)b.Key.DepartmentID, Cost = b.Sum(c => c.ActualPrice), Count = 0 };

                    //获取加1的情况
                    var preStartTimePositiveList = from a in preStartTimeCostList
                                                   where a.Cost == 0
                                                   join b in preEndTimeCostList on a.InPatientID equals b.InPatientID
                                                   where b.Cost > 0
                                                   select new InPatientDepartmentCost { InPatientID = a.InPatientID, DepartmentID = a.DepartmentID, Count = 1 };
                    var preStartTimeNegativeList = from a in preEndTimeCostList
                                                   where a.Cost > 0
                                                   join b in preEndTimeCostList on a.InPatientID equals b.InPatientID
                                                   where b.Cost == 0
                                                   select new InPatientDepartmentCost { InPatientID = a.InPatientID, DepartmentID = a.DepartmentID, Count = -1 };

                    var preStartTimeAntibioticPersonList = new List<InPatientDepartmentCost>();
                    preStartTimeAntibioticPersonList.AddRange(preStartTimePositiveList);
                    preStartTimeAntibioticPersonList.AddRange(preStartTimeNegativeList);
                    var preStartTimeAntibioticPerson = from a in preStartTimeAntibioticPersonList
                                                       group a by a.DepartmentID into b
                                                       select new InPatientDepartmentCost { DepartmentID = b.Key, Count = b.Sum(c => c.Count) };
                    if (preStartTimeAntibioticPerson.Count() > 0)
                    {
                        antibioticPersonList.AddRange(preStartTimeAntibioticPerson);
                    }
                }
                //      对antibioticPersonList分科室，整合成抗菌药物有效人数
                var antibioticDepartmentPerson = from a in antibioticPersonList
                                                 group a by a.DepartmentID into b
                                                 select new { DepartmentID = b.Key, AntibioticPerson = b.Sum(c => c.Count) };
                //获取出院人数
                var inpatientRegisterNumberList = this.innerFactory.CreateInPatientOutDepartmentPerson().GetInPatientOutDepartment(startTime, endTime);
                //Join
                
                result = (from departmentPerson in inpatientRegisterNumberList
                          join antibioticPerson in antibioticDepartmentPerson on departmentPerson.DepartmentID equals antibioticPerson.DepartmentID into gj
                          from subgj in gj.DefaultIfEmpty()
                          select new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = departmentPerson.DepartmentName, RegisterPerson = departmentPerson.RegisterPerson, AntibioticPerson = subgj == null ? 0 : (int)subgj.AntibioticPerson }).ToList();
            }
            catch (Exception )
            {

                throw;
            }

            return result;
        }

        public Task<List<DepartmentAntibioticUsageRateDomain>> GetDepartmentAntibioticUsageRateDomainAsync(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

    }
}
