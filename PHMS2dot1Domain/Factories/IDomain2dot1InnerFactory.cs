using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Factories
{
    public interface IDomain2dot1InnerFactory : IDisposable
    {
        IInPatientFromDrugRecords CreateInPatientFromDrugRecords();
    }
}
