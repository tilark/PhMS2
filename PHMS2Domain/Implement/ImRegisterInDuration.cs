using PHMS2Domain.Interface;
using PHMS2Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2Domain.Implement
{
    public class ImRegisterInDuration
    {
        public class GetOutPatientRegisterInDuration : IRegisterInDuration
        {
            PHMS2DomainContext context = null;
            public GetOutPatientRegisterInDuration():this(new PHMS2DomainContext())
            {

            }
            public GetOutPatientRegisterInDuration(PHMS2DomainContext dbContext)
            {
                this.context = dbContext;
            }
            public List<Registers> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = context.Registers.Where(r => r.RegisterCategoryId == 1 && r.ChargeTime >= startTime && r.ChargeTime < endTime).ToList();
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
            // ~GetOutPatientRegisterInDuration() {
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
        public class GetEmergencyRegisterInDuration : IRegisterInDuration
        {
            PHMS2DomainContext context = null;
            public GetEmergencyRegisterInDuration():this(new PHMS2DomainContext())
            {

            }
            public GetEmergencyRegisterInDuration(PHMS2DomainContext dbContext)
            {
                this.context = dbContext;
            }

            public List<Registers> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = context.Registers.Where(r => r.RegisterCategoryId == 2 && r.ChargeTime >= startTime && r.ChargeTime < endTime).ToList();
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
            // ~GetEmergencyRegisterInDuration() {
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
        public class GetOutPatientEmergencyRegisterInDuration : IRegisterInDuration
        {
            PHMS2DomainContext context = null;
            public GetOutPatientEmergencyRegisterInDuration():this(new PHMS2DomainContext())
            {

            }
            public GetOutPatientEmergencyRegisterInDuration(PHMS2DomainContext dbContext)
            {
                this.context = dbContext;
            }
            public List<Registers> GetRegisterInDuration(DateTime startTime, DateTime endTime)
            {

                var result = context.Registers.Where(r => r.ChargeTime >= startTime && r.ChargeTime < endTime).ToList();
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
            // ~GetOutPatientEmergencyRegisterInDuration() {
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