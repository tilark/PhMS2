using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2.Models.ViewModels.Reporter;

namespace PHMS2.Models.ViewModels.Interface
{
    public interface IDrugCategoryRate
    {
        DrugCategoryRate GetDrugCategoryRate(DateTime startTime, DateTime endTime);
    }
}
