using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Interface
{
    public interface IInPatientInDruation
    {
        List<InPatient> GetInPatientInDruation(DateTime startTime, DateTime endTime);

        Task<List<InPatient>> GetInPatientInDruationAsync(DateTime startTime, DateTime endTime);
    }
}
