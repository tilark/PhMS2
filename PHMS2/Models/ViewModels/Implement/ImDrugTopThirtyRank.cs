using ClassViewModelToDomain;
using ClassViewModelToDomain.IFactory;
using ClassViewModelToDomain.Interface;
using PHMS2.Models.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
namespace PHMS2.Models.ViewModel.Implement
{
    public class ImDrugTopThirtyRank : IDrugTopRank
    {
        private readonly IDomainFacotry DomainFactory;

        public ImDrugTopThirtyRank(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public List<DrugTopRank> GetDrugTopRankList(DateTime startTime, DateTime endTime)
        {

            List<DrugTopRank> result = new List<DrugTopRank>();
            result = this.DomainFactory.CreateDrugTopRank(EnumDrugCategory.ALL_DRUG).GetDrugTopRankList(startTime, endTime);
            //var Iprescrition = this.uow.DomainFactory.CreatePrescrtionInDuration();

            //var prescritionList = Iprescrition.GetPrescriptionInDuration(startTime, endTime);
            //var query = prescritionList.SelectMany(opp => opp.GetDrugCost()).GroupBy(opp => new { opp.ProductNumber, opp.ProductName, opp.IsAntibiotic })
            //       .Select(c => new DrugTopRank
            //       {
            //           ProductNumber = c.Key.ProductNumber,
            //           ProductName = c.Key.ProductName,
            //           Cost = c.Sum(su => su.Cost),
            //           IsAntibiotic = c.Key.IsAntibiotic
            //       }).OrderByDescending(o => o.Cost).Take(30);
            ////根据每个productNumber 按doctorName为组，找到其前三名医生及对应金额
            //foreach (var item in query)
            //{
            //    DrugTopRank temp = new DrugTopRank
            //    {
            //        ProductNumber = item.ProductNumber,
            //        ProductName = item.ProductName,
            //        IsAntibiotic = item.IsAntibiotic,
            //        Cost = item.Cost
            //    };
            //    temp.DrugDoctorDepartmentCostList = prescritionList.SelectMany(opp => opp.GetDrugDoctorDepartmentCost(item.ProductNumber)).GroupBy(g => new { g.Doctor, g.Department })
            //       .Select(c => new DrugDoctorDepartmentCost
            //       {
            //           Doctor = c.Key.Doctor,
            //           Department = c.Key.Department,
            //           Cost = c.Sum(su => su.Cost)
            //       }).OrderByDescending(o => o.Cost).Take(3).ToList();
            //    result.Add(temp);
            //}
            return result;


        }
    }
}