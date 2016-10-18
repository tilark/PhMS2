using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using PHMS2.Models.Factories;
namespace PHMS2.NinjectModules
{
    public class ReporterViewFactoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IReporterViewFactory>().To<ReporterViewFactory>();
            Bind<ReporterViewFactory>().ToSelf().InSingletonScope();
        }
    }
}