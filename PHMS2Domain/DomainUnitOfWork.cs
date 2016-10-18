using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2Domain.Models;
using PHMS2Domain.Factory;

namespace PHMS2Domain
{
    public class DomainUnitOfWork
    {
        //PHMS2DomainContext context = null;
        public DomainUnitOfWork()
        {
            //this.context = new PHMS2DomainContext();
            DomainFactories = new DomainInnerFactory();
        }
        //用于单元测试
        public DomainUnitOfWork(IDomainInnerFactory factory)
        {
            this.DomainFactories = factory;
        }
        public IDomainInnerFactory DomainFactories
        {
            get;
            private set;
        }
        ~DomainUnitOfWork()
        {
            if (this.DomainFactories != null)
            {
                this.DomainFactories.Dispose();
            }
        }
    }
}
