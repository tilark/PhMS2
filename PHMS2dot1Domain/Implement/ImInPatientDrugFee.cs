using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;
using System.Data.Entity;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientDrugFee : IInPatientDrugFee
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatientDrugFee(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public async Task<List<DrugFee>> GetInPatientDrugRecordAsyc(DateTime startTime, DateTime endTime, Guid inPatientDrugRecordID)
        {
            return await this.context.DrugFees.Where(a => a.InPatientDrugRecordID == inPatientDrugRecordID && a.ChargeTime >= startTime && a.ChargeTime < endTime).ToListAsync();
        }
    }
}
