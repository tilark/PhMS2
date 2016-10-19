using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.ViewModels.Implement;
using PHMS2.NinjectModules.InPatientReporterController;
using PhMS2dot1Domain.Factories;
using PHMS2.Models.Factories;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.App_Start
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;
        public NinjectDependencyResolver()
        {
            this.kernel = new StandardKernel();
            this.AddBindings();
        }

        private void AddBindings()
        {
            //PhMS2dot1Domain Bind Factory
            this.kernel.Bind<IDomainFacotry>().To<Domain2dot1OuterFactory>();
            this.kernel.Bind<Domain2dot1OuterFactory>().ToSelf().InSingletonScope();
            this.kernel.Bind<IDomain2dot1InnerFactory>().To<Domain2dot1InnerFactory>();

            //InPatientReporterController Bind factory
            this.kernel.Bind<IInPatientReporterFactory>().To<InPatientReporterFactory>();
            this.kernel.Bind<InPatientReporterFactory>().ToSelf().InSingletonScope();

            //ReporterController
            this.kernel.Bind<IReporterViewFactory>().To<ReporterViewFactory>();
            this.kernel.Bind<ReporterViewFactory>().ToSelf().InSingletonScope();
            

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