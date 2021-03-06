﻿using System;
using System.Collections.Generic;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.Factories;
using PHMS2Domain.Interface;
using System.Linq;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImTopTenAntibioticDepRank : IDrugTopRank
    {
        private readonly IDomainFacotry DomainFactory;

        public ImTopTenAntibioticDepRank(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }

        public List<DrugTopRank> GetDrugTopRankList(DateTime startTime, DateTime endTime)
        {
           
                List<DrugTopRank> result = new List<DrugTopRank>();
            //取定时间范围内的处方单
            result = this.DomainFactory.CreateDrugTopRank(EnumDrugCategory.ANTIBIOTIC_DRUG_DEP).GetDrugTopRankList(startTime, endTime);

            //IPrescriptionInDuration iPrescrtiptions = this.uow.DomainFactory.CreatePrescrtionInDuration();
            //var prescritionList = iPrescrtiptions.GetPrescriptionInDuration(startTime, endTime);
            //var query = prescritionList.SelectMany(opp => opp.GetAntibioticCost()).GroupBy(opp => new { opp.ProductNumber, opp.ProductName, opp.IsAntibiotic })
            //       .Select(c => new DrugTopRank
            //       {
            //           ProductNumber = c.Key.ProductNumber,
            //           ProductName = c.Key.ProductName,
            //           Cost = c.Sum(su => su.Cost),
            //           IsAntibiotic = c.Key.IsAntibiotic
            //       }).OrderByDescending(o => o.Cost).Take(10);
            ////根据每个productNumber 按Department为组，统找到其前三名科室及对应金额
            //foreach (var item in query)
            //{
            //    DrugTopRank temp = new DrugTopRank
            //    {
            //        ProductNumber = item.ProductNumber,
            //        ProductName = item.ProductName,
            //        IsAntibiotic = item.IsAntibiotic,
            //        Cost = item.Cost
            //    };
            //    temp.DrugDoctorDepartmentCostList = prescritionList.SelectMany(opp => opp.GetDrugDoctorDepartmentCost(item.ProductNumber)).GroupBy(g => g.Department)
            //       .Select(c => new DrugDoctorDepartmentCost
            //       {
            //           Department = c.Key,
            //           Cost = c.Sum(su => su.Cost)
            //       }).OrderByDescending(o => o.Cost).Take(3).ToList();
            //    result.Add(temp);
            //}
            return result;
            
               
        }
    }
}