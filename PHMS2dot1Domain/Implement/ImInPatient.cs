using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatient : IInPatient
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatient(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public InPatient GetInPatient(Guid inPatientID)
        {
            InPatient result = null;
            bool getData = true;
            do
            {
                try
                {
                    result = this.context.InPatients.Include("InPatientDrugRecords.DrugFees").Where(i => i.InPatientID == inPatientID).FirstOrDefault();
                    getData = false;
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException)
                {

                }
                catch (Exception)
                {

                    throw;
                }

            } while (getData);


            return result;
        }
    }
}
