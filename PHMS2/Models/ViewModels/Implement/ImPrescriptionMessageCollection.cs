using System;
using System.Linq;
using PHMS2Domain.Interface;
using PHMS2.Models.ViewModel.Interface;
using PHMS2Domain;
using PHMS2Domain.Factory;
using PHMS2.Models.Factories;
using ClassViewModelToDomain;

namespace PHMS2.Models.ViewModel.Implement
{
    public class ImPrescriptionMessageCollection : IPrescriptionMessageCollection
    {
        DomainFactoryUnitOfWork uow = null;
        public ImPrescriptionMessageCollection() : this(new DomainFactoryUnitOfWork())
        {

        }
        public ImPrescriptionMessageCollection(DomainFactoryUnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
        }
        public PrescriptionMessageCollection GetPrescriptionMessageCollection(DateTime startTime, DateTime endTime)
        {

            PrescriptionMessageCollection result = new PrescriptionMessageCollection();
            result.OutPatientMessage = this.uow.DomainFactory.CreatePrescriptionMessage(EnumOutPatientCategories.OUTPATIENT).GetPrescriptionMessage(startTime, endTime);

            result.EmergencyMessage= this.uow.DomainFactory.CreatePrescriptionMessage(EnumOutPatientCategories.EMERGEMENT).GetPrescriptionMessage(startTime, endTime);
            
            result.TotalMessage = new PrescriptionMessage
            {
                InjectAntibioticPerson = result.OutPatientMessage.InjectAntibioticPerson + result.EmergencyMessage.InjectAntibioticPerson,
                UseDrugPerson = result.OutPatientMessage.UseDrugPerson + result.EmergencyMessage.UseDrugPerson,
                AntibioticCategoryNumber = result.OutPatientMessage.AntibioticCategoryNumber + result.EmergencyMessage.AntibioticCategoryNumber,
                AntibioticCost = result.OutPatientMessage.AntibioticCost + result.EmergencyMessage.AntibioticCost,
                DrugCost = result.OutPatientMessage.DrugCost + result.EmergencyMessage.DrugCost
            };
            return result;


        }


    }
}