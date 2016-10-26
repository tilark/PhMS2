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
        #region 住院信息

        
        IDepartmentAntibioticIntensityDomain CreateDepartmentAntibioticIntensityDomain();
        IDepartmentAntibioticUsageRateDomain CreateDepartmentAntibioticUsageRateDomain();
        ISpecialAntibioticDdds CreateSpecialAntibioticDdds();
        ITotalAntibioticDdds CreateTotalAntibioticDdds();
        IDepartmentEssentialUsageRateDomain CreateDepartmentEssentialUsageRateDomain();
        IAntibioticCategoryNumber CreateInPatientAntibioticCategoryNumber();
        IInPatientAntibioticCost CreateInPatientAntibioticCost();
        IPatientCost CreateInPatientDrugCost();

        //住院同期抗菌药物使用人数
        IAntibioticPerson CreateInPatientAntibioticPerson();
        IUnionAntibioticPerson CreateUnionAntibioticPerson();
        #endregion


        #region 门诊信息
        IRegisterPerson CreateRegisterPerson(EnumOutPatientCategories categories);
        IAntibioticPerson CreateAntibioticPerson(EnumOutPatientCategories categories);

        IEssentialDrugCategoryNumbers CreateEssentialDrugCategoryNumbers();
        IDrugCategoriesNumbers CreateDrugCategoriesNumbers();
        IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory);

        IPatientCost CreatePatientCost(EnumOutPatientCategories categories);
        IPrescriptionMessage CreatePrescriptionMessage(EnumOutPatientCategories categories);
        #endregion


    }
}
