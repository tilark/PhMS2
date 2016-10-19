using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Implement
{
    public class ImOutPatientFromDrugRecordInDuration
    {
        public class GetOutPatientRegisterFromPrescription : IOutPatientInDuration
        {
            private readonly PhMS2dot1DomainContext context;

            public GetOutPatientRegisterFromPrescription(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }
            /// <summary>
            /// 取定时间段的处方单所对应的门诊挂号信息.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="endTime">The end time.</param>
            /// <returns>System.Collections.Generic.List&lt;PhMS.Models.Domain.Registers&gt;.</returns>

            public List<OutPatient> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = this.context.OutPatients.Where(r => r.Origin_GHLB.HasValue && r.Origin_GHLB == 1 && r.OutPatientPrescriptions.Any(opp => opp.ChargeTime >= startTime && opp.ChargeTime < endTime)).ToList();
                return result;
            }
        }

        public class GetEmergencyRegisterFromPrescription : IOutPatientInDuration
        {
            private readonly PhMS2dot1DomainContext context;

            public GetEmergencyRegisterFromPrescription(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }
            /// <summary>
            /// 取定时间段的处方单所对应的急诊挂号信息.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="endTime">The end time.</param>
            /// <returns>System.Collections.Generic.List&lt;PhMS.Models.Domain.OutPatient&gt;.</returns>

            public List<OutPatient> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = this.context.OutPatients.Where(r => r.Origin_GHLB.HasValue && r.Origin_GHLB == 2 && r.OutPatientPrescriptions.Any(opp => opp.ChargeTime >= startTime && opp.ChargeTime < endTime)).ToList();
                return result;

            }
        }

        public class GetOutPatientEmergencyRegisterFromPrescription : IOutPatientInDuration
        {
            private readonly PhMS2dot1DomainContext context;

            public GetOutPatientEmergencyRegisterFromPrescription(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }
            /// <summary>
            /// 获取取定时间段内的处方单所对应的挂号信息
            /// 该挂号信息可能在取定时间段内，也有可能在取定时间段前
            /// 未区分门、急
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="endTime">The end time.</param>
            /// <returns>List&lt;Registers&gt;.</returns>
            /// <exception cref="System.NotImplementedException"></exception>
            public List<OutPatient> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = this.context.OutPatients.Where(r => r.OutPatientPrescriptions.Any(opp => opp.ChargeTime >= startTime && opp.ChargeTime < endTime)).ToList();
                return result;
            }
        }
    }
}
