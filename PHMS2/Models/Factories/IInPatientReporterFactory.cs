using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2.Models.Factories
{
    public interface IInPatientReporterFactory
    {
        IDepartmentAntibioticUsageRateList CreateDepartmentAntibioticUsageRateList();
        IDepartmentAntibioticIntensity CreateDepartmentAntibioticIntensity();
        ISpecialAntibioticUsageRate CreateSpecialAntibioticUsageRate();
        IDepartmentEssentialUsageRate CreateDepartmentEssentialUsageRate();
        IInPatientDrugMessage CreateInPatientDrugMessage();
        IInPatientAverageAntibioticCategoryRate CreateInPatientAverageAntibioticCategoryRate();
        IInPatientAverageAntibioticCostRate CreateInPatientAverageAntibioticCostRate();
        IInPatientAntibioticUsageRate CreateInPatientAntibioticUsageRate();
    }
}
