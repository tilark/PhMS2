using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientAntibioticCategoryNumber : IAntibioticCategoryNumber
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientAntibioticCategoryNumber(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public int GetAntibioticCategoryNumber(DateTime startTime, DateTime endTime)
        {
            int result = 0;
            try
            {
                //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);

                var antibioticPositive = inPatientFromDrugRecordList.SelectMany(i => i.DrugCategoryNumberPositiveList(startTime, endTime, ClassViewModelToDomain.EnumDrugCategory.ANTIBIOTIC_DRUG)).Distinct().Count();
                var antibioticNegative = inPatientFromDrugRecordList.SelectMany(i => i.DrugCategoryNumberNegativeList(startTime, endTime, ClassViewModelToDomain.EnumDrugCategory.ANTIBIOTIC_DRUG)).Distinct().Count();

                result = antibioticPositive - antibioticNegative;
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("读取数据库出错! {0}", e.Message));
            }
           
            return result;
        }
    }
}
