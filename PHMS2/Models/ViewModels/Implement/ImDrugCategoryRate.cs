using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.Reporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImDrugCategoryRate : IDrugCategoryRate
    {
        private readonly IDomainFacotry DomainFactory;

        public ImDrugCategoryRate(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public DrugCategoryRate GetDrugCategoryRate(DateTime startTime, DateTime endTime)
        {
            var result = new DrugCategoryRate
            {
                DrugCategoryNums = this.DomainFactory.CreateDrugCategoriesNumbers().GetDrugCategoriesNumbers(startTime, endTime),
                RegisterPersons = this.DomainFactory.CreateRegisterPerson(ClassViewModelToDomain.EnumOutPatientCategories.OUTPATIENT_EMERGEMENT).GetRegisterPerson(startTime, endTime)
            };
            return result;
        }
    }
}