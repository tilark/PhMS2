using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain.Interface;
using PHMS2.Models.ViewModels.Interface;
using ClassViewModelToDomain;

namespace PHMS2.Models.Factories
{
    public interface IReporterViewFactory
    {
        IEssentialDrugRate CreateEssentialDrugRate();
        IAntibioticUsageRate CreateAntibioticUsageRate();
        IDrugTopRank CreateDrugTopRank(EnumDrugCategory drugCategory);
        IPrescriptionMessageCollection CreatePrescriptionMessageCollection();
        IPatientAverageCost CreatePatientAverageCost(EnumOutPatientCategories category);
        IDrugCategoryRate CreateDrugCategoryRate();

    }
}
