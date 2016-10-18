using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain.Interface
{
    public interface IPrescriptionMessage
    {
        PrescriptionMessage GetPrescriptionMessage(DateTime startTime, DateTime endTime);
    }
}
