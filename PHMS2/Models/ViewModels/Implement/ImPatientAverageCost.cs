using System;
using System.Linq;
using PHMS2Domain.Interface;
using PHMS2Domain;
using PHMS2.Models.ViewModel.Interface;
using PHMS2.Models.Factories;
using PHMS2Domain.Factory;
using ClassViewModelToDomain;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModel.Implement
{
    public class ImPatientAverageCost
    {
        public class ImOutPatientAverageCost : IPatientAverageCost
        {
            private readonly IDomainFacotry DomainFactory;

            public ImOutPatientAverageCost(IDomainFacotry factory)
            {
                this.DomainFactory = factory;
            }

            public PatientAverageCost GetOutPatientAverageCost(DateTime startTime, DateTime endTime)
            {
                var result = new PatientAverageCost();
                result = new PatientAverageCost
                {
                    PatientCost = this.DomainFactory.CreatePatientCost(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetPatientCost(startTime, endTime),
                    RegisterPerson = this.DomainFactory.CreateRegisterPerson(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetRegisterPerson(startTime, endTime)
                };             
                return result;
            }
        }
    }
}