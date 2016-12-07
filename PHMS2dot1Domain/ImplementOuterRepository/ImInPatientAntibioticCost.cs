using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientAntibioticCost : IInPatientAntibioticCost
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientAntibioticCost(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }

        public decimal GetInPatientAntibioticCost(DateTime startTime, DateTime endTime)
        {
            decimal result = Decimal.Zero;
            try
            {
                //获取科室及所有药物费用
                result = this.innerFactory.CreateInPatientAllDrugRecordFee().GetInpatientDrugRecordFees(startTime, endTime).Where(w => w.IsAntibiotic == true).Sum(a => a.ActualPrice);               
               
            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }
    }
}
