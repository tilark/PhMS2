using PHMS2Domain.Interface;
using PHMS2Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2Domain.Implement
{
    public class ImRegisterFromPerscription
    {

        public class GetOutPatientRegisterFromPrescription : IRegisterInDuration
        {
            PHMS2DomainContext context = null;
            public GetOutPatientRegisterFromPrescription():this(new PHMS2DomainContext())
            {

            }
            public GetOutPatientRegisterFromPrescription(PHMS2DomainContext dbContext)
            {
                this.context = dbContext;
            }
            /// <summary>
            /// 取定时间段的处方单所对应的门诊挂号信息.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="endTime">The end time.</param>
            /// <returns>System.Collections.Generic.List&lt;PhMS.Models.Domain.Registers&gt;.</returns>

            public List<Registers> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = this.context.Registers.Where(r => r.RegisterCategoryId == 1 && r.OutPatientPrescriptions.Any(opp => opp.ChargeTime >= startTime && opp.ChargeTime < endTime)).ToList();
                return result;


            }

            #region IDisposable Support
            private bool disposedValue = false; // 要检测冗余调用

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: 释放托管状态(托管对象)。
                        if(this.context != null)
                        {
                            this.context.Dispose();
                        }
                    }

                    // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                    // TODO: 将大型字段设置为 null。

                    disposedValue = true;
                }
            }

            // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
            // ~GetOutPatientRegisterFromPrescription() {
            //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            //   Dispose(false);
            // }

            // 添加此代码以正确实现可处置模式。
            public void Dispose()
            {
                // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
                Dispose(true);
                // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
                // GC.SuppressFinalize(this);
            }
            #endregion
        }

        public class GetEmergencyRegisterFromPrescription : IRegisterInDuration
        {
            PHMS2DomainContext context = null;
            public GetEmergencyRegisterFromPrescription():this(new PHMS2DomainContext())
            {

            }
            public GetEmergencyRegisterFromPrescription(PHMS2DomainContext dbContext)
            {
                this.context = dbContext;
            }
            /// <summary>
            /// 取定时间段的处方单所对应的急诊挂号信息.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="endTime">The end time.</param>
            /// <returns>System.Collections.Generic.List&lt;PhMS.Models.Domain.Registers&gt;.</returns>

            public List<Registers> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = this.context.Registers.Where(r => r.RegisterCategoryId == 2 && r.OutPatientPrescriptions.Any(opp => opp.ChargeTime >= startTime && opp.ChargeTime < endTime)).ToList();
                return result;

            }

            #region IDisposable Support
            private bool disposedValue = false; // 要检测冗余调用

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: 释放托管状态(托管对象)。
                        if(this.context != null)
                        {
                            this.context.Dispose();
                        }
                    }

                    // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                    // TODO: 将大型字段设置为 null。

                    disposedValue = true;
                }
            }

            // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
            // ~GetEmergencyRegisterFromPrescription() {
            //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            //   Dispose(false);
            // }

            // 添加此代码以正确实现可处置模式。
            public void Dispose()
            {
                // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
                Dispose(true);
                // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
                // GC.SuppressFinalize(this);
            }
            #endregion
        }

        public class GetOutPatientEmergencyRegisterFromPrescription : IRegisterInDuration
        {
            PHMS2DomainContext context = null;
            public GetOutPatientEmergencyRegisterFromPrescription():this(new PHMS2DomainContext())
            {

            }
            public GetOutPatientEmergencyRegisterFromPrescription(PHMS2DomainContext dbContext)
            {
                this.context = dbContext;
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
            public List<Registers> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = this.context.Registers.Where(r => r.OutPatientPrescriptions.Any(opp => opp.ChargeTime >= startTime && opp.ChargeTime < endTime)).ToList();
                return result;

            }

            #region IDisposable Support
            private bool disposedValue = false; // 要检测冗余调用

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: 释放托管状态(托管对象)。
                        if(this.context != null)
                        {
                            this.context.Dispose();
                        }
                    }

                    // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                    // TODO: 将大型字段设置为 null。

                    disposedValue = true;
                }
            }

            // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
            // ~GetOutPatientEmergencyRegisterFromPrescription() {
            //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            //   Dispose(false);
            // }

            // 添加此代码以正确实现可处置模式。
            public void Dispose()
            {
                // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
                Dispose(true);
                // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
                // GC.SuppressFinalize(this);
            }
            #endregion
        }
    }
}