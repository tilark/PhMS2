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
    public class ImGetEssentialDrugCategoryNumbers : IEssentialDrugCategoryNumbers
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImGetEssentialDrugCategoryNumbers(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public int GetEssentialDrugCategoryNumbers(DateTime startTime, DateTime endTime)
        {
            int result = 0;
            try
            {
                //获得取定时间段内处方信息所对应的挂号信息
                var registerInDuration = this.innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT)
                                        .GetRegisterInDuration(startTime, endTime);
                var positiveNumber = registerInDuration.SelectMany(r => r.DrugCategoryNumberPositiveList(startTime, endTime, EnumDrugCategory.ESSENTIAL_DRUG)).Distinct().Count();
                var negaiveNumber = registerInDuration.SelectMany(r => r.DrugCategoryNumberNegativeList(startTime, endTime, EnumDrugCategory.ESSENTIAL_DRUG)).Distinct().Count();
                result = positiveNumber - negaiveNumber;
            }
            catch (Exception)
            {

                //throw new InvalidOperationException(String.Format("读取数据库出错!"));
                throw;
            }

            return result;
        }
    }
}
