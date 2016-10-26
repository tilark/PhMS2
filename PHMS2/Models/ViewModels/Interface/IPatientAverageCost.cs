using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PHMS2.Models.ViewModels.Interface
{
    public interface IPatientAverageCost
    {
        PatientAverageCost GetOutPatientAverageCost(DateTime startTime, DateTime endTime);

    }
}
