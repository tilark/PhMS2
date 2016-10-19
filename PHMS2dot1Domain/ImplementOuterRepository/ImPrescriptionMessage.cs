using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImPrescriptionMessage
    {
        public class ImOutPatientPrescriptionMessage : IPrescriptionMessage
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public ImOutPatientPrescriptionMessage(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public PrescriptionMessage GetPrescriptionMessage(DateTime startTime, DateTime endTime)
            {
                PrescriptionMessage result = new PrescriptionMessage();
                try
                {
                    var registerList = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT).GetRegisterInDuration(startTime, endTime);
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
                    result = null;

                }

                return result;
            }
        }

        public class ImEmergencyPrescriptionMessage : IPrescriptionMessage
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public ImEmergencyPrescriptionMessage(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public PrescriptionMessage GetPrescriptionMessage(DateTime startTime, DateTime endTime)
            {
                PrescriptionMessage result = new PrescriptionMessage();
                try
                {
                    var registerList = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT).GetRegisterInDuration(startTime, endTime);
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

                    result = null;
                }

                return result;
            }
        }

        public class ImOutPatientEmergencyPrescriptionMessage : IPrescriptionMessage
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public ImOutPatientEmergencyPrescriptionMessage(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public PrescriptionMessage GetPrescriptionMessage(DateTime startTime, DateTime endTime)
            {
                PrescriptionMessage result = new PrescriptionMessage();
                try
                {
                    var registerList = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetRegisterInDuration(startTime, endTime);
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

                    result = null;
                }

                return result;
            }
        }
    }
}
