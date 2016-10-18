using PHMS2Domain.Factory;
using PHMS2Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PHMS2Domain.Implement
{
    public class ImAntibioticPerson
    {
        public class GetOutPatientAntibioticPerson : IAntibioticPerson
        {
            DomainUnitOfWork uow = null;

            public GetOutPatientAntibioticPerson()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetOutPatientAntibioticPerson(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
            {
                int result = 0;
                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetAntibioticPersonCount(registersList, startTime, endTime);

                return result;

            }
        }

        public class GetEmergencyAntibioticPerson : IAntibioticPerson
        {
            DomainUnitOfWork uow = null;

            public GetEmergencyAntibioticPerson()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetEmergencyAntibioticPerson(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
            {
                int result = 0;


                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);

                result = new GetCountFromRegisterList().GetAntibioticPersonCount(registersList, startTime, endTime);


                return result;

            }
        }
        public class GetOutPatientEmergencyAntibioticPerson : IAntibioticPerson
        {
            DomainUnitOfWork uow = null;
            public GetOutPatientEmergencyAntibioticPerson()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetOutPatientEmergencyAntibioticPerson(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
            {
                int result = 0;
                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetAntibioticPersonCount(registersList, startTime, endTime);
                return result;
            }
        }
    }
}