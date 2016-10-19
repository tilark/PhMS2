using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;
namespace PhMS2dot1Domain.Interface
{
    public interface IOutPatientInDuration
    {
        List<OutPatient> GetRegisterInDuration(DateTime startTime, DateTime endTime);
    }
}
