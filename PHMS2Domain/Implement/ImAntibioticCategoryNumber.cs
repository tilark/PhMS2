using PHMS2Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2Domain.Factory;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PHMS2Domain.Implement
{
    public class ImAntibioticCategoryNumber
    {

        public class GetEmergencyAntibioticCategoryNumber : IAntibioticCategoryNumber
        {
            DomainUnitOfWork uow = null;

            public GetEmergencyAntibioticCategoryNumber()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetEmergencyAntibioticCategoryNumber(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
            {
                int result = 0;
                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetAntibioticCategoryNumberCount(registersList, startTime, endTime);
                return result;

            }
        }

        public class GetOutPatientAntibioticCategoryNumber : IAntibioticCategoryNumber
        {
            DomainUnitOfWork uow = null;

            public GetOutPatientAntibioticCategoryNumber()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetOutPatientAntibioticCategoryNumber(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
            {
                int result = 0;

                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);

                result = new GetCountFromRegisterList().GetAntibioticCategoryNumberCount(registersList, startTime, endTime);

                return result;
            }
        }

        public class GetOutPatientEmergencyAntibioticCategoryNumber : IAntibioticCategoryNumber
        {
            DomainUnitOfWork uow = null;

            public GetOutPatientEmergencyAntibioticCategoryNumber()
            {
                this.uow = new DomainUnitOfWork();
            }
            public GetOutPatientEmergencyAntibioticCategoryNumber(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
            {
                int result = 0;

                IRegisterInDuration register = this.uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);

                result = new GetCountFromRegisterList().GetAntibioticCategoryNumberCount(registersList, startTime, endTime);

                return result;
            }
        }
    }
}