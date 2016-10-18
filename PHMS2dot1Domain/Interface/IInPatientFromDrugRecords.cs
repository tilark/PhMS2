using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;
namespace PhMS2dot1Domain.Interface
{
    /// <summary>
    /// 接口：通过DrugRecord中的时间点选择出取定时间内的入院病历
    /// </summary>
    public interface IInPatientFromDrugRecords
    {
        List<InPatient> GetInPatientFromDrugRecords(DateTime startTime, DateTime endTime);
    }
}
