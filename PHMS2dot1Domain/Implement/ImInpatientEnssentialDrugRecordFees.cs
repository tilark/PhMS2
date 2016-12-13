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
    public class ImInpatientEnssentialDrugRecordFees : IInPatientDrugRecordDrugFeeView
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInpatientEnssentialDrugRecordFees(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InpatientDrugRecordFees> GetInpatientDrugRecordFees(DateTime startTime, DateTime endTime)
        {
            var result = new List<InpatientDrugRecordFees>();
            try
            {
                result = (from a in this.context.InPatients
                          where a.OutDate.HasValue && !a.CaseNumber.Contains("XT")
                          join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                          where b.IsEssential == true
                          join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                          where (a.OutDate.Value >= startTime && a.OutDate.Value < endTime && c.ChargeTime < endTime) || (a.OutDate.Value < startTime && c.ChargeTime >= startTime && c.ChargeTime < endTime)
                          select new InpatientDrugRecordFees {  DepartmentID = a.Origin_DEPT_ID,  ActualPrice = c.ActualPrice }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
