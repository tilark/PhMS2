using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    public interface IInPatientDrugFee
    {
        Task<List<DrugFee>> GetInPatientDrugRecordAsyc(DateTime startTime, DateTime endTime, Guid inPatientDrugRecordID);
    }
}
