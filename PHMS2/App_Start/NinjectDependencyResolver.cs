using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.ViewModels.Implement;
using PHMS2.NinjectModules.InPatientReporterController;
using PhMS2dot1Domain.Factories;

namespace PHMS2.App_Start
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;
        public NinjectDependencyResolver()
        {
            this.kernel = new StandardKernel(new InPatienReporterModule());
            this.AddBindings();
        }

        private void AddBindings()
        {
            //this.kernel.Bind<IInPatientReporterFactory>().To<InPatientReporterFactory>();
            this.kernel.Bind<IDomain2dot1OuterFactory>().To<Domain2dot1OuterFactory>();
            this.kernel.Bind<Domain2dot1OuterFactory>().ToSelf().InSingletonScope();
            this.kernel.Bind<IDomain2dot1InnerFactory>().To<Domain2dot1InnerFactory>();

        }

        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }


    }
}