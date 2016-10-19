﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModel;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.Factories;
using ClassViewModelToDomain.Interface;
using Ninject;
using PhMS2dot1Domain.Factories;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImDepartmentAntibioticUsageRateList : IDepartmentAntibioticUsageRateList
    {
        private readonly IDomainFacotry factory;

        public ImDepartmentAntibioticUsageRateList(IDomainFacotry factory)
        {
            this.factory = factory;
        }
        public DepartmentAntibioticUsageRate GetDepartmentAntibioticUsageRateList(DateTime startTime, DateTime endTime)
        {
            var result = new DepartmentAntibioticUsageRate();
            
            try
            {
                result.DepartmentAntibioticUsageRateList = this.factory.CreateDepartmentAntibioticUsageRateDomain().GetDepartmentAntibioticUsageRateDomain(startTime, endTime);
            }
            catch (Exception)
            {
                result.DepartmentAntibioticUsageRateList = new List<ClassViewModelToDomain.DepartmentAntibioticUsageRateDomain>();
            }
            return result;
        }
    }
}