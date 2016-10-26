using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImSpecialAntibioticDdds : ISpecialAntibioticDdds
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImSpecialAntibioticDdds(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public decimal GetSpecialAntibioticDdds(DateTime startTime, DateTime endTime)
        {
            decimal result = Decimal.Zero;
            try
            {
                //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);
                result = inPatientFromDrugRecordList.Sum(i => i.SpecialDddInDuration(startTime, endTime));
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("读取数据失败！{0}", e.Message));
            }
            return result;
        }
    }
}
