using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain.Interface;

namespace ClassViewModelToDomain.IFactory
{
    public interface IDomainFacotry
    {
        IDepartmentAntibioticUsageRateDomain CreateDepartmentAntibioticUsageRateDomain();

        //门诊信息
        IRegisterPerson CreateRegisterPerson(EnumOutPatientCategories categories);
        IAntibioticPerson CreateAntibioticPerson(EnumOutPatientCategories categories);

        IEssentialDrugCategoryNumbers CreateEssentialDrugCategoryNumbers();
        IDrugCategoriesNumbers CreateDrugCategoriesNumbers();
        IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory);

        IPatientCost CreatePatientCost(EnumOutPatientCategories categories);
        IPrescriptionMessage CreatePrescriptionMessage(EnumOutPatientCategories categories);
    }
}
