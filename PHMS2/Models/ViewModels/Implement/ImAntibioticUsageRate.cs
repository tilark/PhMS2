using PHMS2.Models.Factories;
using PHMS2Domain;
using System;
using PHMS2.Models.ViewModel.Interface;
using PHMS2Domain.Interface;
using PHMS2Domain.Factory;
using ClassViewModelToDomain;

namespace PHMS2.Models.ViewModel.Implement
{
    public class ImAntibioticUsageRate : IAntibioticUsageRate
    {
        DomainFactoryUnitOfWork uow = null;
        public ImAntibioticUsageRate():this(new DomainFactoryUnitOfWork())
        {

        }
        public ImAntibioticUsageRate(DomainFactoryUnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
        }
        public AntibioticUsageRate GetAntibioticUsageRate(DateTime startTime, DateTime endTime, EnumOutPatientCategories categories)
        {
                
                var result =
                 new AntibioticUsageRate
                 {
                     AntibioticPerson = this.uow.DomainFactory.CreateAntibioticPerson(categories).GetAntibioticPerson(startTime, endTime),
                     RegisterPerson = this.uow.DomainFactory.CreateRegisterPerson(categories).GetRegisterPerson(startTime, endTime)
                 };
                
                return result;
                
        }
    }
}