using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.ViewModels.Implement;
using PhMS2dot1Domain.Factories;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.Factories
{
    public class InPatientReporterFactory : IInPatientReporterFactory
    {
        private readonly IDomainFacotry factory;

        public InPatientReporterFactory(IDomainFacotry factory)
        {
            this.factory = factory;
        }
        public IDepartmentAntibioticUsageRateList CreateDepartmentAntibioticUsageRateList()
        {
            return new ImDepartmentAntibioticUsageRateList(this.factory);
        }
    }
}