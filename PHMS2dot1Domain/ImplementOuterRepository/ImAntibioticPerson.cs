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
    public class ImAntibioticPerson
    {
        public class GetOutPatientAntibioticPerson : IAntibioticPerson
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public GetOutPatientAntibioticPerson(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
            {
                int result = 0;
                var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetAntibioticPersonCount(registersList, startTime, endTime);

                return result;

            }
        }

        public class GetEmergencyAntibioticPerson : IAntibioticPerson
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            private readonly IInnerRepository innerRepository;
            public GetEmergencyAntibioticPerson(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
                this.innerRepository = new InnerRepository(this.innerFactory);
            }
            public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
            {
                int result = 0;


                var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);

                result = new GetCountFromRegisterList().GetAntibioticPersonCount(registersList, startTime, endTime);


                return result;

            }
        }
        public class GetOutPatientEmergencyAntibioticPerson : IAntibioticPerson
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            private readonly IInnerRepository innerRepository;
            public GetOutPatientEmergencyAntibioticPerson(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
                this.innerRepository = new InnerRepository(this.innerFactory);
            }
            public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
            {
                int result = 0;
                var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetAntibioticPersonCount(registersList, startTime, endTime);
                return result;
            }
        }
    }
}
