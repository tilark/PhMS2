using PHMS2Domain;
using PHMS2Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PHMS2.Models.Factories
{
    public interface IReporterDomainFactory
    {
        IPrescriptionInDuration CreatePrescrtionInDuration();
        IRegisterInDuration CreateRegisterFromPrescription(EnumOutPatientCategories categories);
        IRegisterInDuration CreateRegisterInDuration(EnumOutPatientCategories categories);
        IRegisterPerson CreateRegisterPerson(EnumOutPatientCategories categories);
        IAntibioticPerson CreateAntibioticPerson(EnumOutPatientCategories categories);
        IAntibioticCategoryNumber CreateAntibioticCategoryNumber(EnumOutPatientCategories categories);

    }
}
