using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Interface;

namespace PhMS2dot1Domain.Factories
{
    public interface IDomain2dot1InnerFactory : IDisposable
    {
        #region 住院信息

        IInPatientInDruation CreateInPatientFromDrugRecords();
        IInPatientInDruation CreateInPatientInDuration();
        IDepartment CreateDepartment();
        IInPatient CreateInPatient();
        IInPatientDrugRecord CreateInPatientDrugRecord();
        IInPatientDrugFee CreateInPatientDrugFee();
        #endregion

        #region 门诊信息

        #endregion
        IPrescriptionInDuration CreatePrescrtionInDuration();
        IOutPatientInDuration CreateRegisterFromPrescription(EnumOutPatientCategories categories);
        IOutPatientInDuration CreateRegisterInDuration(EnumOutPatientCategories categories);
    }
}
