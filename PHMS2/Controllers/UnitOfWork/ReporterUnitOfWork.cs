using PHMS2.Models.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Controllers.UnitOfWork
{
    public class ReporterUnitOfWork
    {
       
        //用于单元测试
        public ReporterUnitOfWork(IReporterViewFactory factory)
        {
            this.ReporterViewFactory = factory;
        }
        public IReporterViewFactory ReporterViewFactory
        {
            get;
            private set;
        }

       
    }
}