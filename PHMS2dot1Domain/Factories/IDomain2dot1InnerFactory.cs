using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Factories
{
    public interface IDomain2dot1InnerFactory : IDisposable
    {
        PhMS2dot1DomainContext dbContext { get; }
        #region 住院信息
        IInPatientAllDrugCost CreateInPatientAllDrugCost();
        IInPatientAntibioticCostDomain CreateInPatientAntibioticCost();
        IInPatientDepartmentDrugName CreateInPatientDepartmentDrugName();
        //IInPatientDrugRecordDrugFeeView CreateInPatientAntibioticDrugRecordFee();
        //IInPatientDrugRecordDrugFeeView CreateInPatientEssentialDrugRecordFee();
        //IInPatientDrugRecordDrugFeeView CreateInPatientAllDrugRecordFee();
        IInPatientDrugRecordDrugFeesView CreateInPatientDrugRecordFeeView();

        IInPatientOutDepartment CreateInPatientOutDepartmentPerson();
        IInPatientInDruation CreateInPatientFromDrugRecords();
        IInPatientInDruation CreateInPatientInDuration();
        IDepartment CreateDepartment();


        //IInPatient CreateInPatient();
        //IInPatientDrugRecord CreateInPatientDrugRecord();
        //IInPatientDrugFee CreateInPatientDrugFee();
        #endregion

        #region 门诊信息

        #endregion
        IPrescriptionInDuration CreatePrescrtionInDuration();
        IOutPatientInDuration CreateRegisterFromPrescription(EnumOutPatientCategories categories);
        IOutPatientInDuration CreateRegisterInDuration(EnumOutPatientCategories categories);
    }
}
