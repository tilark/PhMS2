using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientFromDrugRecords
    {
        /// <summary>
        /// 实现IInPatientFromDrugRecords，根据药物记录中的收费时间找出入院病人病例
        /// </summary>
        public class ImGetInPatientFromDrugRecords : IInPatientFromDrugRecords
        {
            private readonly PhMS2dot1DomainContext context;

            public ImGetInPatientFromDrugRecords(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }
            public List<InPatient> GetInPatientFromDrugRecords(DateTime startTime, DateTime endTime)
            {
                var result = new List<InPatient>();
                result = this.context.InPatients.Where(i => i.InPatientDrugRecords.Any(ii => ii.DrugFees.Any(df => df.ChargeTime >= startTime && df.ChargeTime < endTime))).ToList();
                return result;
            }
        }
    }
}
