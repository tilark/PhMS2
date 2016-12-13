using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Implement
{
    public class ImInpatientAllDrugRecordFees : IInPatientDrugRecordDrugFeeView
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInpatientAllDrugRecordFees(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InpatientDrugRecordFees> GetInpatientDrugRecordFees(DateTime startTime, DateTime endTime)
        {
            var result = new List<InpatientDrugRecordFees>();
            try
            {
                //var sw = new StreamWriter(@"e:\databaseInPatientAntibioticLog.log") { AutoFlush = true };
                //this.context.Database.Log = s => { sw.Write(s); };
                result = (from a in this.context.InPatients
                          where a.OutDate.HasValue  && !a.CaseNumber.StartsWith("XT")
                          join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                          join d in this.context.AntibioticLevels on b.Origin_KSSDJID equals d.Origin_KSSDJID
                          join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                          where (a.OutDate.Value >= startTime && a.OutDate.Value < endTime && c.ChargeTime < endTime) || (a.OutDate.Value < startTime && c.ChargeTime >= startTime && c.ChargeTime < endTime)
                          select new InpatientDrugRecordFees {  DepartmentID = a.Origin_DEPT_ID,  ActualPrice = c.ActualPrice, IsEssential = b.IsEssential, IsAntibiotic = d.IsAntibiotic }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
