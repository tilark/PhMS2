using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2Domain.Interface;
using PHMS2Domain.Implement;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PHMS2Domain.Factory
{
    public interface IDomainOuterFactory : IDisposable
    {
        //IPrescriptionInDuration CreatePrescrtionInDuration();
        //IRegisterInDuration CreateRegisterFromPrescription(EnumOutPatientCategories categories);
        //IRegisterInDuration CreateRegisterInDuration(EnumOutPatientCategories categories);
        IRegisterPerson CreateRegisterPerson(EnumOutPatientCategories categories);
        IAntibioticPerson CreateAntibioticPerson(EnumOutPatientCategories categories);

        IEssentialDrugCategoryNumbers CreateEssentialDrugCategoryNumbers();
        IDrugCategoriesNumbers CreateDrugCategoriesNumbers();
        IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory);

        IPatientCost CreatePatientCost(EnumOutPatientCategories categories);
        IPrescriptionMessage CreatePrescriptionMessage(EnumOutPatientCategories categories);
    }
}
