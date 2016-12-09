using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImAntibioticCategoryNumber
    {

        public class GetEmergencyAntibioticCategoryNumber : IAntibioticCategoryNumber
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public GetEmergencyAntibioticCategoryNumber(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
            {
                int result = 0;
                try
                {
                    var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.EMERGEMENT);
                    var registersList = register.GetRegisterInDuration(startTime, endTime);
                    result = new GetCountFromRegisterList().GetAntibioticCategoryNumberCount(registersList, startTime, endTime);

                }
                catch (Exception)
                {

                    throw;
                }
                return result;
            }
        }

        public class GetOutPatientAntibioticCategoryNumber : IAntibioticCategoryNumber
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public GetOutPatientAntibioticCategoryNumber(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
            {
                int result = 0;

                var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);
                result = new GetCountFromRegisterList().GetAntibioticCategoryNumberCount(registersList, startTime, endTime);

                return result;
            }
        }

        public class GetOutPatientEmergencyAntibioticCategoryNumber : IAntibioticCategoryNumber
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public GetOutPatientEmergencyAntibioticCategoryNumber(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
            {
                int result = 0;

                var register = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT);
                var registersList = register.GetRegisterInDuration(startTime, endTime);

                result = new GetCountFromRegisterList().GetAntibioticCategoryNumberCount(registersList, startTime, endTime);

                return result;
            }
        }
    }
}
