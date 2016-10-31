using PHMS2.Models.ViewModels.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2.Models.ViewModels.Interface
{
    public interface IDrugTopThirtyDescription
    {
        DrugTopThirtyDescription GetDrugTopThirtyDescription(DateTime startTime, DateTime endTime);
    }
}
