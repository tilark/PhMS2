﻿using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    public interface IInPatientDrugRecord
    {
        List<InPatientDrugRecord> GetInPatientDrugRecords(DateTime startTime, DateTime endTime, Guid inPatientID);
        List<InPatientDrugRecord> GetInPatientDrugRecordsAsync(Guid inPatientID);
    }
}
