using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImTopTenAntibioticRank : IDrugTopRank
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImTopTenAntibioticRank(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }

        public List<DrugTopRank> GetDrugTopRankList(DateTime startTime, DateTime endTime)
        {

            List<DrugTopRank> result = new List<DrugTopRank>();
            //取定时间范围内的处方单
            var iPrescrtiptions = this.innerFactory.CreatePrescrtionInDuration();
            var prescritionList = iPrescrtiptions.GetPrescriptionInDuration(startTime, endTime);
            var query = prescritionList.SelectMany(opp => opp.GetAntibioticCost()).GroupBy(opp => new { opp.ProductCJID, opp.ProductName, opp.IsAntibiotic })
                   .Select(c => new DrugTopRank
                   {
                       ProductCJID = c.Key.ProductCJID,
                       ProductName = c.Key.ProductName,
                       Cost = c.Sum(su => su.Cost),
                       IsAntibiotic = c.Key.IsAntibiotic
                   }).OrderByDescending(o => o.Cost).Take(10);
            //根据每个productNumber 按doctorName为组，统找到其前三名医生及对应金额
            foreach (var item in query)
            {
                DrugTopRank temp = new DrugTopRank
                {
                    ProductCJID = item.ProductCJID,
                    ProductName = item.ProductName,
                    IsAntibiotic = item.IsAntibiotic,
                    Cost = item.Cost
                };
                temp.DrugDoctorDepartmentCostList = prescritionList.SelectMany(opp => opp.GetDrugDoctorDepartmentCost(item.ProductNumber)).GroupBy(g => new { g.DoctorID, g.DepartmentID })
                   .Select(c => new DrugDoctorDepartmentCost
                   {
                       DoctorID = c.Key.DoctorID,
                       DepartmentID = c.Key.DepartmentID,
                       Cost = c.Sum(su => su.Cost)
                   }).OrderByDescending(o => o.Cost).Take(3).ToList();
                result.Add(temp);
            }
            return result;


        }
    }
}
