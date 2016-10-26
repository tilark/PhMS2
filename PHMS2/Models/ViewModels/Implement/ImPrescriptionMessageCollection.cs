using System;
using System.Linq;
using PHMS2Domain.Interface;
using PHMS2.Models.ViewModels.Interface;
using PHMS2Domain;
using PHMS2Domain.Factory;
using PHMS2.Models.Factories;
using ClassViewModelToDomain;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImPrescriptionMessageCollection : IPrescriptionMessageCollection
    {
        private readonly IDomainFacotry DomainFactory;

        public ImPrescriptionMessageCollection(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public PrescriptionMessageCollection GetPrescriptionMessageCollection(DateTime startTime, DateTime endTime)
        {

            PrescriptionMessageCollection result = new PrescriptionMessageCollection();
            result.OutPatientMessage = this.DomainFactory.CreatePrescriptionMessage(EnumOutPatientCategories.OUTPATIENT).GetPrescriptionMessage(startTime, endTime);

            result.EmergencyMessage= this.DomainFactory.CreatePrescriptionMessage(EnumOutPatientCategories.EMERGEMENT).GetPrescriptionMessage(startTime, endTime);
            
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