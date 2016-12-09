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
    public class ImInPatientDepartmentDrugName : IInPatientDepartmentDrugName
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatientDepartmentDrugName(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InPatientDepartmentDrugName> GetInPatientDepartmentDrugName(DateTime startTime, DateTime endTime)
        {
            var result = new List<InPatientDepartmentDrugName>();
            try
            {
                result = (from a in this.context.InPatients
                          where a.OutDate.HasValue && a.OutDate.Value < endTime && !a.CaseNumber.Contains("XT")
                          join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                          join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                          where c.ChargeTime >= startTime && c.ChargeTime < endTime
                          select new InPatientDepartmentDrugName { InPatientID = a.InPatientID, InDate = a.InDate, OutDate = a.OutDate, DepartmentID = a.Origin_DEPT_ID, Origin_CJID = b.Origin_CJID, ProductName = b.ProductName, ChargeTime = c.ChargeTime,   Quantity = c.Quantity,  ActualPrice = c.ActualPrice }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
