using ClassViewModelToDomain.IFactory;
using PHMS2Domain.Factory;
using PhMS2dot1Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.Factories
{
    public class DomainFactoryUnitOfWork : IDisposable
    {
        #region  PhMSDomain

        //public DomainFactoryUnitOfWork()
        //{
        //    //PhMSDomain
        //   this.DomainFactory = new DomainOuterFactory();
        //    //PhMS2dot2Domain
        //}
        ////用于单元测试
        //public DomainFactoryUnitOfWork(IDomainOuterFactory factory)
        //{
        //    this.DomainFactory = factory;
        //}

        //public IDomainOuterFactory DomainFactory
        //{
        //    get;
        //    private set;
        //}
        #endregion

        public DomainFactoryUnitOfWork()
        {
            
        }
        public DomainFactoryUnitOfWork(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public IDomainFacotry DomainFactory
        {
            get;
            private set;
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
                    if (this.DomainFactory != null)
                    {
                        //this.DomainFactory.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DomainFactoryUnitOfWork() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}