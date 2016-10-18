﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;
using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Implement;

namespace PhMS2dot1Domain.Factories
{
    public class Domain2dot1InnerFactory : IDomain2dot1InnerFactory
    {
        private readonly  PhMS2dot1DomainContext context = null;
        #region Structer
        public Domain2dot1InnerFactory() : this(new PhMS2dot1DomainContext())
        {

        }
        public Domain2dot1InnerFactory(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        #endregion

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
        // ~Domain2dot1InnerFactory() {
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
        public IInPatientFromDrugRecords CreateInPatientFromDrugRecords()
        {
            return new ImInPatientFromDrugRecords.ImGetInPatientFromDrugRecords(this.context);
        }


    }
}
