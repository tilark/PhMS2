using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Implement
{
    public class ImOutPatientInDuration
    {
        /// <summary>
        /// 获取门诊挂号
        /// </summary>
        public class GetOutPatientRegisterInDuration : IOutPatientInDuration
        {
            private readonly PhMS2dot1DomainContext context;

            public GetOutPatientRegisterInDuration(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }

            public List<OutPatient> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {
                var result = this.context.OutPatients.Where(r => r.Origin_GHLB.HasValue && r.Origin_GHLB == 1 && r.ChargeTime >= startTime && r.ChargeTime < endTime).ToList();
                return result;
            }
        }
        /// <summary>
        /// 获取急诊挂号病人信息
        /// </summary>
        public class GetEmergencyRegisterInDuration : IOutPatientInDuration
        {
            private readonly PhMS2dot1DomainContext context;

            public GetEmergencyRegisterInDuration(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }

            public List<OutPatient> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = this.context.OutPatients.Where(r => r.Origin_GHLB.HasValue && r.Origin_GHLB == 2 && r.ChargeTime >= startTime && r.ChargeTime < endTime).ToList();
                return result;

            }
        }
        /// <summary>
        /// 获取门、急诊挂号信息
        /// </summary>
        public class GetOutPatientEmergencyRegisterInDuration : IOutPatientInDuration
        {
            private readonly PhMS2dot1DomainContext context;

            public GetOutPatientEmergencyRegisterInDuration(PhMS2dot1DomainContext context)
            {
                this.context = context;
            }
            public List<OutPatient> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = context.OutPatients.Where(r => r.ChargeTime >= startTime && r.ChargeTime < endTime).ToList();
                return result;

            }
        }
    }
    
}

