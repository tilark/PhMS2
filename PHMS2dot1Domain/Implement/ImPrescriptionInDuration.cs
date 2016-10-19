using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Implement
{
    public class ImPrescriptionInDuration 
    {
        public class GetPrescriptionInDurationList : IPrescriptionInDuration
        {
            private readonly PhMS2dot1DomainContext context;

            public GetPrescriptionInDurationList(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }
            public List<OutPatientPrescription> GetPrescriptionInDuration(DateTime startTime, DateTime endTime)
            {

                return context.OutPatientPrescriptions.Where(opp => opp.ChargeTime >= startTime && opp.ChargeTime < endTime)
                   .ToList();

            }
        }
    }
}
