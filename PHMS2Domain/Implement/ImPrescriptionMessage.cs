using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;

namespace PHMS2Domain.Implement
{
    public class ImPrescriptionMessage
    {
        public class ImOutPatientPrescriptionMessage : IPrescriptionMessage
        {
            DomainUnitOfWork uow = null;

            public ImOutPatientPrescriptionMessage()
            {
                this.uow = new DomainUnitOfWork();
            }
            public ImOutPatientPrescriptionMessage(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public PrescriptionMessage GetPrescriptionMessage(DateTime startTime, DateTime endTime)
            {
                PrescriptionMessage result = new PrescriptionMessage();
                try
                {
                    var registerList = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT).GetRegisterInDuration(startTime, endTime);
                    result = new PrescriptionMessage
                    {
                        InjectAntibioticPerson = 0,
                        UseDrugPerson = registerList.Sum(r => r.DrugPerson(startTime, endTime)),
                        AntibioticCategoryNumber = registerList.SelectMany(r => r.AntibioticCategoryNumberList(startTime, endTime)).Distinct().ToList().Count,
                        AntibioticCost = Decimal.Round(registerList.Sum(r => r.AntibioticCost(startTime, endTime)), 2),
                        DrugCost = Decimal.Round(registerList.Sum(r => r.DrugCost(startTime, endTime)), 2)
                    };
                }
                catch (Exception)
                {


                }

                return result;
            }
        }

        public class ImEmergencyPrescriptionMessage : IPrescriptionMessage
        {
            DomainUnitOfWork uow = null;

            public ImEmergencyPrescriptionMessage()
            {
                this.uow = new DomainUnitOfWork();
            }
            public ImEmergencyPrescriptionMessage(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public PrescriptionMessage GetPrescriptionMessage(DateTime startTime, DateTime endTime)
            {
                PrescriptionMessage result = new PrescriptionMessage();
                try
                {
                    var registerList = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT).GetRegisterInDuration(startTime, endTime);
                    result = new PrescriptionMessage
                    {
                        InjectAntibioticPerson = 0,
                        UseDrugPerson = registerList.Sum(r => r.DrugPerson(startTime, endTime)),
                        AntibioticCategoryNumber = registerList.SelectMany(r => r.AntibioticCategoryNumberList(startTime, endTime)).Distinct().ToList().Count,
                        AntibioticCost = Decimal.Round(registerList.Sum(r => r.AntibioticCost(startTime, endTime)), 2),
                        DrugCost = Decimal.Round(registerList.Sum(r => r.DrugCost(startTime, endTime)), 2)
                    };
                }
                catch (Exception)
                {


                }

                return result;
            }
        }

        public class ImOutPatientEmergencyPrescriptionMessage : IPrescriptionMessage
        {
            DomainUnitOfWork uow = null;

            public ImOutPatientEmergencyPrescriptionMessage()
            {
                this.uow = new DomainUnitOfWork();
            }
            public ImOutPatientEmergencyPrescriptionMessage(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public PrescriptionMessage GetPrescriptionMessage(DateTime startTime, DateTime endTime)
            {
                PrescriptionMessage result = new PrescriptionMessage();
                try
                {
                    var registerList = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetRegisterInDuration(startTime, endTime);
                    result = new PrescriptionMessage
                    {
                        InjectAntibioticPerson = 0,
                        UseDrugPerson = registerList.Sum(r => r.DrugPerson(startTime, endTime)),
                        AntibioticCategoryNumber = registerList.SelectMany(r => r.AntibioticCategoryNumberList(startTime, endTime)).Distinct().ToList().Count,
                        AntibioticCost = Decimal.Round(registerList.Sum(r => r.AntibioticCost(startTime, endTime)), 2),
                        DrugCost = Decimal.Round(registerList.Sum(r => r.DrugCost(startTime, endTime)), 2)
                    };
                }
                catch (Exception)
                {


                }

                return result;
            }
        }
    }
}
