using PHMS2.Models.Factories;
using PHMS2Domain;
using System;
using PHMS2.Models.ViewModels.Interface;
using PHMS2Domain.Interface;
using PHMS2Domain.Factory;
using ClassViewModelToDomain;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImAntibioticUsageRate : IAntibioticUsageRate
    {
        private readonly IDomainFacotry DomainFactory;

        public ImAntibioticUsageRate(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public AntibioticUsageRate GetAntibioticUsageRate(DateTime startTime, DateTime endTime, EnumOutPatientCategories categories)
        {
                
                var result =
                 new AntibioticUsageRate
                 {
                     AntibioticPerson = this.DomainFactory.CreateAntibioticPerson(categories).GetAntibioticPerson(startTime, endTime),
                     RegisterPerson = this.DomainFactory.CreateRegisterPerson(categories).GetRegisterPerson(startTime, endTime)
                 };
                
                return result;
                
        }
    }
}