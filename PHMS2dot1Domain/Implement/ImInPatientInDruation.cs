using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;
using System.Data.Entity;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientInDruation
    {
        /// <summary>
        /// 返回出院时间在取定时间段内的住院病例集合.
        /// </summary>
        public class ImGetInPatientInDruation : IInPatientInDruation
        {
            private readonly PhMS2dot1DomainContext context;

            public ImGetInPatientInDruation(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }

            public List<InPatient> GetInPatientInDruation(DateTime startTime, DateTime endTime)
            {
                return this.context.InPatients.Where(i => i.OutDate.HasValue && i.OutDate.Value >= startTime && i.OutDate.Value < endTime && !i.CaseNumber.Contains("XT")).ToList();
            }

            public async Task<List<InPatient>> GetInPatientInDruationAsync(DateTime startTime, DateTime endTime)
            {
                return await this.context.InPatients.Where(i => i.OutDate.HasValue && i.OutDate.Value >= startTime && i.OutDate.Value < endTime && !i.CaseNumber.Contains("XT")).ToListAsync();
            }
        }

        /// <summary>
        /// 实现IInPatientFromDrugRecords，根据药物记录中的收费时间找出入院病人病例
        /// InPatient中的OutDate不为NULL，表明是已出院患者，并且DrugFee中收费时间在取定时间段内，可包含退费病人。
        /// </summary>
        public class ImGetInPatientFromDrugRecords : IInPatientInDruation
        {
            private readonly PhMS2dot1DomainContext context;

            public ImGetInPatientFromDrugRecords(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }
            public List<InPatient> GetInPatientInDruation(DateTime startTime, DateTime endTime)
            {
                var result = new List<InPatient>();
                //result = this.context.InPatients.Where(i => i.OutDate.HasValue && i.InPatientDrugRecords.Any(ii => ii.DrugFees.Any(df => df.ChargeTime >= startTime && df.ChargeTime < endTime))).ToList();
                //取定有出院时间，出院时间小于endTime，在取定时间段内有交费记录的。
                result = this.context.InPatients.Where(i => i.OutDate.HasValue && i.OutDate.Value < endTime && !i.CaseNumber.Contains("XT") &&  i.InPatientDrugRecords.Any(ii => ii.DrugFees.Any(df => df.ChargeTime >= startTime && df.ChargeTime < endTime))).ToList();
                return result;
            }

            public async Task<List<InPatient>> GetInPatientInDruationAsync(DateTime startTime, DateTime endTime)
            {
                return await this.context.InPatients.Where(i => i.OutDate.HasValue && i.OutDate.Value < endTime && !i.CaseNumber.Contains("XT") && i.InPatientDrugRecords.Any(ii => ii.DrugFees.Any(df => df.ChargeTime >= startTime && df.ChargeTime < endTime))).ToListAsync();
            }
        }
    }
}
