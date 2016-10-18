using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using PHMS2.Models.Factories;

namespace PHMSNinjectModule.ReporterControllerModule
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
