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
    public class ImRegisterPerson
    {
        public class GetEmergencyRegisterPerson : IRegisterPerson
        {
            DomainUnitOfWork uow = null;

            public GetEmergencyRegisterPerson()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetEmergencyRegisterPerson(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetRegisterPerson(DateTime startTime, DateTime endTime)
            {

                int result = 0;

                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetRegisterPersonCount(registersList, startTime, endTime);

                return result;
            }
        }

        public class GetOutPatientEmergencyRegisterPerson : IRegisterPerson
        {
            DomainUnitOfWork uow = null;

            public GetOutPatientEmergencyRegisterPerson()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetOutPatientEmergencyRegisterPerson(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetRegisterPerson(DateTime startTime, DateTime endTime)
            {


                int result = 0;
                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetRegisterPersonCount(registersList, startTime, endTime);
                return result;


            }
        }

        public class GetOutPatientRegisterPerson : IRegisterPerson
        {
            DomainUnitOfWork uow = null;

            public GetOutPatientRegisterPerson()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetOutPatientRegisterPerson(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetRegisterPerson(DateTime startTime, DateTime endTime)
            {

                int result = 0;
                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterInDuration(EnumOutPatientCategories.OUTPATIENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetRegisterPersonCount(registersList, startTime, endTime);
                return result;
            }

        }
    }
}
