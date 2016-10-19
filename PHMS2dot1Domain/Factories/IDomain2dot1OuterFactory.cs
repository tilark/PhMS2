using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PhMS2dot1Domain.Factories
{
    public interface IDomain2dot1OuterFactory
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
