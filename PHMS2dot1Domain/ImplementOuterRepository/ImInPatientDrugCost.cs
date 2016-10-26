using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientDrugCost : IPatientCost
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientDrugCost(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public decimal GetPatientCost(DateTime startTime, DateTime endTime)
        {
            Decimal result = Decimal.Zero;
            try
            {
                //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);

                result = inPatientFromDrugRecordList.Sum(i => i.GetTotalDrugCost(startTime, endTime));
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("读取数据出错! {0}", e.Message));
            }
            
            return result;
        }
    }
}
