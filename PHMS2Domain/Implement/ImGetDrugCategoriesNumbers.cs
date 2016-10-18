using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2Domain.Interface;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PHMS2Domain.Implement
{
    public class ImGetDrugCategoriesNumbers : IDrugCategoriesNumbers
    {
        DomainUnitOfWork uow = null;

        public ImGetDrugCategoriesNumbers()
        {
            this.uow = new DomainUnitOfWork();
        }
        public ImGetDrugCategoriesNumbers(DomainUnitOfWork uow)
        {
            this.uow = uow;
        }
        public int GetDrugCategoriesNumbers(DateTime startTime, DateTime endTime)
        {
            int result = 0;
            try
            {
                //获得取定时间段内处方信息所对应的挂号信息
                var registerInDuration = uow.DomainFactories.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT)
                                        .GetRegisterInDuration(startTime, endTime);
                var positiveNumber = registerInDuration.SelectMany(r => r.DrugCategoryNumberPositiveList(startTime, endTime, EnumDrugCategories.ALL_DRUG)).Distinct().Count();
                var negaiveNumber = registerInDuration.SelectMany(r => r.DrugCategoryNumberNegativeList(startTime, endTime, EnumDrugCategories.ALL_DRUG)).Distinct().Count();
                result = positiveNumber - negaiveNumber;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("读取数据库出错！");

            }
           
            return result;
        }
    }
}
