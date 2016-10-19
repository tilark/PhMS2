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
    public class ImRegisterPerson
    {
        public class GetEmergencyRegisterPerson : IRegisterPerson
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public GetEmergencyRegisterPerson(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public int GetRegisterPerson(DateTime startTime, DateTime endTime)
            {

                int result = 0;

                var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetRegisterPersonCount(registersList, startTime, endTime);

                return result;
            }
        }

        public class GetOutPatientEmergencyRegisterPerson : IRegisterPerson
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public GetOutPatientEmergencyRegisterPerson(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public int GetRegisterPerson(DateTime startTime, DateTime endTime)
            {


                int result = 0;
                var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetRegisterPersonCount(registersList, startTime, endTime);
                return result;


            }
        }

        public class GetOutPatientRegisterPerson : IRegisterPerson
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public GetOutPatientRegisterPerson(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public int GetRegisterPerson(DateTime startTime, DateTime endTime)
            {
                int result = 0;
                var register = this.innerFactory.CreateRegisterInDuration(EnumOutPatientCategories.OUTPATIENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetRegisterPersonCount(registersList, startTime, endTime);
                return result;
            }
        }
    }
}
