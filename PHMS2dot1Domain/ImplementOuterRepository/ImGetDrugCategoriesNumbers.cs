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
    public class ImGetDrugCategoriesNumbers : IDrugCategoriesNumbers
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImGetDrugCategoriesNumbers(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public int GetDrugCategoriesNumbers(DateTime startTime, DateTime endTime)
        {
            int result = 0;
            try
            {
                //获得取定时间段内处方信息所对应的挂号信息
                var registerInDuration = innerFactory.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT)
                                        .GetRegisterInDuration(startTime, endTime);
                var positiveNumber = registerInDuration.SelectMany(r => r.DrugCategoryNumberPositiveList(startTime, endTime, EnumDrugCategory.ALL_DRUG)).Distinct().Count();
                var negaiveNumber = registerInDuration.SelectMany(r => r.DrugCategoryNumberNegativeList(startTime, endTime, EnumDrugCategory.ALL_DRUG)).Distinct().Count();
                result = positiveNumber - negaiveNumber;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("读取数据库出错！", e);

            }

            return result;
        }
    }
}
