using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientAntibioticCost : IInPatientAntibioticCostDomain
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatientAntibioticCost(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }

        public decimal GetInPatientAntibioticCost(DateTime startTime, DateTime endTime)
        {
            decimal result = 0M;
            try
            {
                result = (from a in this.context.InPatients
                          where a.OutDate.HasValue && a.OutDate.Value < endTime && !a.CaseNumber.Contains("XT")
                          join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                          join d in this.context.AntibioticLevels on b.Origin_KSSDJID equals d.Origin_KSSDJID
                          where d.IsAntibiotic == true
                          join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                          where c.ChargeTime >= startTime && c.ChargeTime < endTime
                          select new { Cost = c.ActualPrice }).Sum( d => d.Cost); 
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
