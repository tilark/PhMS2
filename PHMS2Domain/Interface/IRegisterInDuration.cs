using PHMS2Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PHMS2Domain.Interface
{
    public interface IRegisterInDuration : IDisposable
    {
        List<Registers> GetRegisterInDuration( DateTime startTime, DateTime endTime);

    }
}
