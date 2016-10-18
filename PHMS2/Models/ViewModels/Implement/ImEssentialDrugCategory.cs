using System;
using System.Linq;
using PHMS2Domain.Interface;
using PHMS2.Models.ViewModel.Interface;
using PHMS2.Models.Factories;
using PHMS2Domain.Factory;

namespace PHMS2.Models.ViewModel.Implement
{
    public class ImEssentialDrugCategory : IEssentialDrugRate
    {

        DomainFactoryUnitOfWork uow = null;
        public ImEssentialDrugCategory():this(new DomainFactoryUnitOfWork())
        {

        }
        public ImEssentialDrugCategory(DomainFactoryUnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
        }
        /// <summary>
        /// 获取基本药物品种率.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ViewModel.EssentialDrugCategoryRate.</returns>
        /// <remarks>从取定时间段内的处方表中获取每个基本药物品种的代码集合，去重后得出品种数
        /// </remarks>
        public ViewModel.EssentialDrugCategoryRate GetEssentialDrugCategoryRate( DateTime startTime, DateTime endTime)
        {
            
            var result = new EssentialDrugCategoryRate
            {
                EssentialDrugNums = this.uow.DomainFactory.CreateEssentialDrugCategoryNumbers().GetEssentialDrugCategoryNumbers(startTime, endTime),
                DrugCategoriesNums = this.uow.DomainFactory.CreateDrugCategoriesNumbers().GetDrugCategoriesNumbers(startTime, endTime)
            };
            
            return result;
        }

    }
    public class GetEssentialDrugCategoryTestData : IEssentialDrugRate
    {
        
        public EssentialDrugCategoryRate GetEssentialDrugCategoryRate(DateTime startTime, DateTime endTime)
        {
            var result = new EssentialDrugCategoryRate
            {
                EssentialDrugNums = 80,
                DrugCategoriesNums = 120
            };
           
            return result;
        }
    }
}