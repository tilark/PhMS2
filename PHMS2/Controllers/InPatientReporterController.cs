using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PHMS2.Models.ViewModel;
using Ninject;
using PHMS2.NinjectModules.InPatientReporterController;
using PHMS2.Models.ViewModels.Interface;
using ClassViewModelToDomain;
using PHMS2.Models.Factories;

namespace PHMS2.Controllers
{
    public class InPatientReporterController : Controller
    {
        private readonly IInPatientReporterFactory factory;

        public InPatientReporterController(IInPatientReporterFactory factory)
        {
            this.factory = factory;
        }
        // GET: InPatientReporter
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InPatientAntibioticUsageRate(DateTime startTime, DateTime endTime)
        {
            var viewModel = new DepartmentAntibioticUsageRate();
            viewModel.DepartmentAntibioticUsageRateList = new List<ClassViewModelToDomain.DepartmentAntibioticUsageRateDomain>();
            try
            {
                viewModel = this.factory.CreateDepartmentAntibioticUsageRateList().GetDepartmentAntibioticUsageRateList(startTime, endTime);
            }
            catch (Exception)
            {
                var tempData = new DepartmentAntibioticUsageRateDomain
                {
                    AntibioticPerson = 0,
                    RegisterPerson = 0,
                    DepartmentID = 0,
                    DepartmentName = "Empty"
                };
                
                viewModel.DepartmentAntibioticUsageRateList.Add(tempData);
            }
            //return PartialView("_InPatientAntibioticUsageRate", viewModel);
            return View(viewModel);
        }
    }
}