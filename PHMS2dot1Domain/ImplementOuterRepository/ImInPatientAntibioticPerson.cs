using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientAntibioticPerson : IAntibioticPerson
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientAntibioticPerson(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
        {
            int result = 0;
            try
            {
                //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);

                var personPositive = inPatientFromDrugRecordList.Sum(a => a.AntibioticPersonPositive(startTime, endTime));
                var personNegative = inPatientFromDrugRecordList.Sum(a => a.AntibioticPersonNegative(startTime, endTime));
                result = personPositive - personNegative;
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("读取数据库出错! {0}", e.Message));
            }
            return result;
        }
    }
}
