using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2Domain.Implement
{
    public class ImPatientCost 
    {
        public class ImGetOutPatientEmergencyCost: IPatientCost
        {
            DomainUnitOfWork uow = null;

            public ImGetOutPatientEmergencyCost()
            {
                this.uow = new DomainUnitOfWork();
            }
            public ImGetOutPatientEmergencyCost(DomainUnitOfWork uow)
            {
                this.uow = uow;
            }
            //public class GetOutPatient
            public decimal GetPatientCost(DateTime startTime, DateTime endTime)
            {
                decimal result = 0M;
                var registerList = this.uow.DomainFactories.CreateRegisterInDuration(ClassViewModelToDomain.EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetRegisterInDuration(startTime, endTime);
                result = Decimal.Round( registerList.Sum(reg => reg.DrugCost(startTime, endTime)), 2);
                return result;
            }
        }
        
    }
}
