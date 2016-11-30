using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientDrugRecord : IInPatientDrugRecord
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatientDrugRecord(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InPatientDrugRecord> GetInPatientDrugRecords(DateTime startTime, DateTime endTime,Guid inPatientID)
        {
            var result = new List<InPatientDrugRecord>();
            int tryTimes = 0;
            do
            {
                tryTimes++;
                try
                {
                    result = this.context.InPatientDrugRecords.Include("DrugFees").Where(a => a.InPatientID == inPatientID).ToList();
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException)
                {
                    //getData = false;
                }
                catch (System.Data.Entity.Core.EntityException)
                {

                }
                catch (Exception)
                {
                    //getData = false;
                    //throw;
                }
            } while (tryTimes < 11);

            return result;
        }

        public  IQueryable<InPatientDrugRecord> GetInPatientDrugRecordsAsync( Guid inPatientID)
        {
            return  this.context.InPatientDrugRecords.Where(a => a.InPatientID == inPatientID);
        }

        List<InPatientDrugRecord> IInPatientDrugRecord.GetInPatientDrugRecordsAsync(Guid inPatientID)
        {
            throw new NotImplementedException();
        }
    }
}
