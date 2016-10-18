using System;
using System.Linq;
using PHMS2Domain.Interface;
using PHMS2Domain;
using PHMS2.Models.ViewModel.Interface;
using PHMS2.Models.Factories;
using PHMS2Domain.Factory;
using ClassViewModelToDomain;

namespace PHMS2.Models.ViewModel.Implement
{
    public class ImPatientAverageCost
    {
        public class ImOutPatientAverageCost : IPatientAverageCost
        {
            DomainFactoryUnitOfWork uow = null;
            public ImOutPatientAverageCost() : this(new DomainFactoryUnitOfWork())
            {

            }
            public ImOutPatientAverageCost(DomainFactoryUnitOfWork unitOfWork)
            {
                this.uow = unitOfWork;
            }

            public PatientAverageCost GetOutPatientAverageCost(DateTime startTime, DateTime endTime)
            {
                var result = new PatientAverageCost();
                result = new PatientAverageCost
                {
                    PatientCost = this.uow.DomainFactory.CreatePatientCost(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetPatientCost(startTime, endTime),
                    RegisterPerson = this.uow.DomainFactory.CreateRegisterPerson(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetRegisterPerson(startTime, endTime)
                };             
                return result;
            }
        }
    }
}