using ClassViewModelToDomain.IFactory;
using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImDepartmentEssentialUsageRate : IDepartmentEssentialUsageRate
    {
        private readonly IDomainFacotry factory;

        public ImDepartmentEssentialUsageRate(IDomainFacotry factory)
        {
            this.factory = factory;
        }

        public DepartmentEssentialUsageRate GetDepartmentEssentialUsageRate(DateTime startTime, DateTime endTime)
        {
            var result = new DepartmentEssentialUsageRate();
            try
            {
                result.DepartmentEssentialUsageRateList = this.factory.CreateDepartmentEssentialUsageRateDomain().GetDepartmentEssentialUsageRateDomain(startTime, endTime);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(String.Format("读取数据错误！{0}", e.Message));
            }
            return result;
        }
    }
}