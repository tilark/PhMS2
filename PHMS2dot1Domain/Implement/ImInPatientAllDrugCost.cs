using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientAllDrugCost : IInPatientAllDrugCost
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatientAllDrugCost(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public decimal GetInPatientAllDrugCost(DateTime startTime, DateTime endTime)
        {
            decimal result = 0M;
            try
            {
                result = (from a in this.context.InPatients
                          where a.OutDate.HasValue && !a.CaseNumber.Contains("XT")
                          join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                          join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                          where c.ChargeTime >= startTime && c.ChargeTime < endTime
                          select new { Cost = c.ActualPrice }).Sum(d => d.Cost);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
