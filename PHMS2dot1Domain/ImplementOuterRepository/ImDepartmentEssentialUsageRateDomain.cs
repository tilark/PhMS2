using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    /// <summary>
    /// Class ImDepartmentEssentialUsageRateDomain.获取科室基本药物使用率集合
    /// </summary>
    public class ImDepartmentEssentialUsageRateDomain : IDepartmentEssentialUsageRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        private readonly IInnerRepository innerRepository;
        public ImDepartmentEssentialUsageRateDomain(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
            this.innerRepository = new InnerRepository(this.innerFactory);
        }
        public List<DepartmentEssentialUsageRateDomain> GetDepartmentEssentialUsageRateDomain(DateTime startTime, DateTime endTime)
        {
            var result = new List<DepartmentEssentialUsageRateDomain>();
            try
            {
                //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);
                //获取同期基本药物使用科室及金额
                var departmentEssentialCost = inPatientFromDrugRecordList.SelectMany(i => i.GetDepartmentEssentialDrugCost(startTime, endTime)).GroupBy(a => a.DepartmentID).Select(g => new { DepartmentID = g.Key, EssentialCost = g.Sum(b => b.Cost) }).ToList();
                //获取同期药物使用科室及金额
                var totalDrugCost = inPatientFromDrugRecordList.SelectMany(i => i.GetDepartmentTotalDrugCost(startTime, endTime)).GroupBy(a => a.DepartmentID).Select(g => new { DepartmentID = g.Key, TotalCost = g.Sum(b => b.Cost) }).ToList();
                //获取科室集合
                var departments = this.innerFactory.CreateDepartment().GetDepartment();

                //Join
                var essentialJoinDepartmentName = departmentEssentialCost.Join(departments, essential => essential.DepartmentID, dep => dep.DepartmentID, (essential, dep) => new { DepartmentID = essential.DepartmentID, DepartmentName = dep.DepartmentName, EssentialCost = essential.EssentialCost }).ToList();

                result = essentialJoinDepartmentName.Join(totalDrugCost, essential => essential.DepartmentID, totalDrug => totalDrug.DepartmentID, (essential, totalDrug) => new DepartmentEssentialUsageRateDomain { DepartmentID = essential.DepartmentID, DepartmentName = essential.DepartmentName, EssentialCost = essential.EssentialCost, TotalDrugCost = totalDrug.TotalCost }).ToList();

            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据处理出错!{0}", e.Message));
            }
            return result;
        }
    }
}
