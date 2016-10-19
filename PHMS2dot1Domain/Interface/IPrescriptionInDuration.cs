using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    public interface IPrescriptionInDuration
    {
        List<OutPatientPrescription> GetPrescriptionInDuration(DateTime startTime, DateTime endTime);
    }
}
