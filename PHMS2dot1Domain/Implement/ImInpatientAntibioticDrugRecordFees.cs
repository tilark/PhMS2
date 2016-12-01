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
    public class ImInpatientAntibioticDrugRecordFees : IInPatientDrugRecordDrugFeeView
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInpatientAntibioticDrugRecordFees(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InpatientDrugRecordFees> GetInpatientDrugRecordFees(DateTime startTime, DateTime endTime)
        {
            var result = new List<InpatientDrugRecordFees>();
            try
            {
                result = (from a in this.context.InPatients
                          where a.OutDate.HasValue && a.OutDate.Value < endTime
                          join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                          join d in this.context.AntibioticLevels on b.Origin_KSSDJ equals d.Origin_KSSDJ
                          where d.IsAntibiotic == true
                          join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                          where c.ChargeTime >= startTime && c.ChargeTime < endTime
                          select new InpatientDrugRecordFees { InPatientID = a.InPatientID, InDate = a.InDate, OutDate = a.OutDate, KSSDJ = b.Origin_KSSDJ, DepartmentID = a.Origin_DEPT_ID, ChargeTime = c.ChargeTime, ActualPrice = c.ActualPrice, Quantity = c.Quantity, DDD = b.DDD }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return result;

        }
    }
}
