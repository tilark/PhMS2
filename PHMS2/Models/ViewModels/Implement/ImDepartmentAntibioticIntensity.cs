using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImDepartmentAntibioticIntensity : IDepartmentAntibioticIntensity
    {
        private readonly IDomainFacotry factory;

        public ImDepartmentAntibioticIntensity(IDomainFacotry factory)
        {
            this.factory = factory;
        }
        public DepartmentAntibioticIntensity GetDepartmentAntibioticIntensity(DateTime startTime, DateTime endTime)
        {
            var result = new DepartmentAntibioticIntensity();
            try
            {
                result.DepartmentAntibioticIntensityList = this.factory.CreateDepartmentAntibioticIntensityDomain().GetDepartmentAntibioticIntensityDomain(startTime, endTime);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("读取数据错误！{0}", e.Message);
            }
            return result;
        }
    }
}