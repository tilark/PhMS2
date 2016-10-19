using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImPatientCost
    {
        public class ImGetOutPatientEmergencyCost : IPatientCost
        {
            private readonly IDomain2dot1InnerFactory innerFactory;
            public ImGetOutPatientEmergencyCost(IDomain2dot1InnerFactory factory)
            {
                this.innerFactory = factory;
            }
            //public class GetOutPatient
            public decimal GetPatientCost(DateTime startTime, DateTime endTime)
            {
                decimal result = 0M;
                var registerList = this.innerFactory.CreateRegisterInDuration(ClassViewModelToDomain.EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetRegisterInDuration(startTime, endTime);
                result = Decimal.Round(registerList.Sum(reg => reg.DrugCost(startTime, endTime)), 2);
                return result;
            }
        }

    }
}
