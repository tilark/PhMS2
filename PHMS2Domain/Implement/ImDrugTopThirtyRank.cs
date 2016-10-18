﻿using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;

namespace PHMS2Domain.Implement
{
    /// <summary>
    /// ImDrugTopThirtyRank，获得金额总额为使用前三十的药品，所对应的前三个医生及对应金额
    /// </summary>
    public class ImDrugTopThirtyRank : IDrugTopRank
    {
       
        DomainUnitOfWork uow = null;

        public ImDrugTopThirtyRank()
        {
            this.uow = new DomainUnitOfWork();
        }
        public ImDrugTopThirtyRank(DomainUnitOfWork uow)
        {
            this.uow = uow;
        }

        public List<DrugTopRank> GetDrugTopRankList(DateTime startTime, DateTime endTime)
        {
            List<DrugTopRank> result = new List<DrugTopRank>();

            var Iprescrition = this.uow.DomainFactories.CreatePrescrtionInDuration();

            var prescritionList = Iprescrition.GetPrescriptionInDuration(startTime, endTime);
            var query = prescritionList.SelectMany(opp => opp.GetDrugCost()).GroupBy(opp => new { opp.ProductNumber, opp.ProductName, opp.IsAntibiotic })
                   .Select(c => new DrugTopRank
                   {
                       ProductNumber = c.Key.ProductNumber,
                       ProductName = c.Key.ProductName,
                       Cost = c.Sum(su => su.Cost),
                       IsAntibiotic = c.Key.IsAntibiotic
                   }).OrderByDescending(o => o.Cost).Take(30);
            //根据每个productNumber 按doctorName为组，找到其前三名医生及对应金额
            foreach (var item in query)
            {
                DrugTopRank temp = new DrugTopRank
                {
                    ProductNumber = item.ProductNumber,
                    ProductName = item.ProductName,
                    IsAntibiotic = item.IsAntibiotic,
                    Cost = item.Cost
                };
                temp.DrugDoctorDepartmentCostList = prescritionList.SelectMany(opp => opp.GetDrugDoctorDepartmentCost(item.ProductNumber)).GroupBy(g => new { g.Doctor, g.Department })
                   .Select(c => new DrugDoctorDepartmentCost
                   {
                       Doctor = c.Key.Doctor,
                       Department = c.Key.Department,
                       Cost = c.Sum(su => su.Cost)
                   }).OrderByDescending(o => o.Cost).Take(3).ToList();
                result.Add(temp);
            }
            return result;
        }
    }
}
