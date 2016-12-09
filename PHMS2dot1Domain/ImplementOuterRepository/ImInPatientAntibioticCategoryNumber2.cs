using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.ViewModels;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    class ImInPatientAntibioticCategoryNumber2 : IAntibioticCategoryNumber
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientAntibioticCategoryNumber2(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
        {
            int result = 0;
            try
            {
                //获取inPatient、DrugRecord、DrugFee关于药物种类的基本信息
                var inPatientDepartmentDrug = this.innerFactory.CreateInPatientAntibioticDrugRecordFee().GetInpatientDrugRecordFees(startTime, endTime);
                //antibioticCategoryList 集合包括取定时间段内的科室与抗菌药物有效人数和在开始时间段前的抗菌药物有效人数
                var antibioticCategoryList = new List<InPatientDepartmentCost>();
                //分成两部分，一部分在取定时间段内，另一部分为在开始时间段前（出院后仍产生费用部分）
                //取定时间段的部分，按照病人，科室，药物种类，筛选出消费费用大于0的情况
                var inDurationList = from a in inPatientDepartmentDrug
                                     where a.OutDate >= startTime
                                     group a by new { a.InPatientID, a.DepartmentID, a.Origin_CJID } into b
                                     where b.Sum(c => c.ActualPrice) > 0
                                     select new InPatientDepartmentCost { InPatientID = b.Key.InPatientID, DepartmentID = (int)b.Key.DepartmentID, Origin_CJID = b.Key.Origin_CJID };
                //再根据大于0的情况，按CJID分组，计算出CJID的总数，即出院患者使用抗菌药物的种类总数
                var inDurationDrugCategory = from a in inDurationList
                                             group a by a.Origin_CJID into b
                                             select new InPatientDepartmentCost {  Origin_CJID = b.Key, Count = b.Count() };
                antibioticCategoryList.AddRange(inDurationDrugCategory);

                //另一部分为出院病人在startTime之前已出院，但是在取定时间段内还有费用产生，如退费等
                var preStartTimeList = inPatientDepartmentDrug.Where(a => a.OutDate.Value < startTime).AsParallel().ToList();
                if(preStartTimeList.Count > 0)
                {
                    //获取住院时间与startTime之间的所有费用，即<startTime的总费用
                    var preStartTimeCostList = from a in preStartTimeList
                                               where a.InDate < startTime
                                               group a by new { a.InPatientID, a.DepartmentID, a.Origin_CJID } into b
                                               select new InPatientDepartmentCost { InPatientID = b.Key.InPatientID, DepartmentID = (int)b.Key.DepartmentID, Origin_CJID = b.Key.Origin_CJID, Cost = b.Sum(c => c.ActualPrice), Count = 0 };
                    //获取住院时间与endTime之间的所有费用，即<endTime的总费用，用来判断是加1还是减1，还是为0
                    var preEndTimeCostList = from a in preStartTimeList
                                             where a.InDate < endTime
                                             group a by new { a.InPatientID, a.DepartmentID, a.Origin_CJID } into b
                                             select new InPatientDepartmentCost { InPatientID = b.Key.InPatientID, DepartmentID = (int)b.Key.DepartmentID, Origin_CJID = b.Key.Origin_CJID, Cost = b.Sum(c => c.ActualPrice), Count = 0 };

                    //获取加1的情况
                    var preStartTimePositiveList = from a in preStartTimeCostList
                                                   where a.Cost == 0
                                                   join b in preEndTimeCostList on a.InPatientID equals b.InPatientID
                                                   where b.Cost > 0
                                                   select new InPatientDepartmentCost { InPatientID = a.InPatientID, DepartmentID = a.DepartmentID,Origin_CJID = a.Origin_CJID, Count = 1 };
                    var preStartTimeNegativeList = from a in preEndTimeCostList
                                                   where a.Cost > 0
                                                   join b in preEndTimeCostList on a.InPatientID equals b.InPatientID
                                                   where b.Cost == 0
                                                   select new InPatientDepartmentCost { InPatientID = a.InPatientID, DepartmentID = a.DepartmentID,Origin_CJID = a.Origin_CJID, Count = -1 };

                    var preStartTimeAntibioticCategoryList = new List<InPatientDepartmentCost>();
                    preStartTimeAntibioticCategoryList.AddRange(preStartTimePositiveList);
                    preStartTimeAntibioticCategoryList.AddRange(preStartTimeNegativeList);
                    var preStartTimeAntibioticCategory = from a in preStartTimeAntibioticCategoryList
                                                       group a by a.Origin_CJID into b
                                                       select new InPatientDepartmentCost { Origin_CJID = b.Key, Count = b.Sum(c => c.Count) };
                    if (preStartTimeAntibioticCategory.Count() > 0)
                    {
                        antibioticCategoryList.AddRange(preStartTimeAntibioticCategory);
                    }
                }

                //将所有的抗菌药物列表集合，按CJID分组，计算出总数
                result = ((from a in antibioticCategoryList
                                group a by a.Origin_CJID into b
                                where b.Sum(c => c.Count) > 0
                                select b.Count())).ToList().Count;

                
                         
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
