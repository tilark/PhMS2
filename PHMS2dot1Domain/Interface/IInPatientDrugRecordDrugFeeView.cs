using PhMS2dot1Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    public interface IInPatientDrugRecordDrugFeeView
    {
        List<InpatientDrugRecordDrugFeesView> GetInpatientDrugRecordFees(DateTime startTime, DateTime endTime);
    }
}
