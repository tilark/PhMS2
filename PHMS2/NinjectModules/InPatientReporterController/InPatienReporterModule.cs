using Ninject.Modules;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.Factories;
using PHMS2.Models.ViewModels.Implement;
using PHMS2.Models.ViewModels.Interface;
namespace PHMS2.NinjectModules.InPatientReporterController
{
    public class InPatienReporterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IInPatientReporterFactory>().To<InPatientReporterFactory>();
            Bind<InPatientReporterFactory>().ToSelf().InSingletonScope();
        }
    }
}