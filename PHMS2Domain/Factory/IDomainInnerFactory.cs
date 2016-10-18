using ClassViewModelToDomain;
using PHMS2Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2Domain.Factory
{
    public interface IDomainInnerFactory : IDisposable
    {
        IPrescriptionInDuration CreatePrescrtionInDuration();
        IRegisterInDuration CreateRegisterFromPrescription(EnumOutPatientCategories categories);
        IRegisterInDuration CreateRegisterInDuration(EnumOutPatientCategories categories);
    }
}
