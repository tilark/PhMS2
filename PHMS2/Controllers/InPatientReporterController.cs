using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using PHMS2.Models.ViewModels;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.Factories;
using ClassViewModelToDomain;
using PHMS2.Models.ViewModels.InPatientReporter;
using System.Threading.Tasks;

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

        #region 抗菌药物

        /// <summary>
        /// 住院患者抗菌药物使用率（分院科两级指标）.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> InPatientDepartmentAntibioticUsageRate(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new DepartmentAntibioticUsageRate();
            viewModel.DepartmentAntibioticUsageRateList = new List<ClassViewModelToDomain.DepartmentAntibioticUsageRateDomain>();
            try
            {
                viewModel = await this.factory.CreateDepartmentAntibioticUsageRateList().GetDepartmentAntibioticUsageRateListAsync(startTime, endTime);
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
            return PartialView("_InPatientAntibioticUsageRate", viewModel);
            //return View(viewModel);
        }
        /// <summary>
        /// 抗菌药物使用强度（分院科两级指标）.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult InPatientAntibioticIntensity(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new DepartmentAntibioticIntensity();
            viewModel.DepartmentAntibioticIntensityList = new List<DepartmentAntibioticIntensityDomain>();
            try
            {
                viewModel = this.factory.CreateDepartmentAntibioticIntensity().GetDepartmentAntibioticIntensity(startTime, endTime);
            }
            catch (Exception)
            {
                var temp = new DepartmentAntibioticIntensityDomain
                {
                    DepartmentID = -1,
                    DepartmentName = "Empty",
                    AntibioticDdd = -1,
                    PersonNumberDays = 0

                };
                viewModel.DepartmentAntibioticIntensityList.Add(temp);
            }
            return PartialView("_GetInPatientAntibioticIntensity", viewModel);

        }
        /// <summary>
        /// 住院科室特殊使用级抗菌药物使用量占比.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult InPatientSpecialAntibioticUsageRate(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new SpecialAntibioticUsageRate();
            try
            {
                viewModel = this.factory.CreateSpecialAntibioticUsageRate().GetSpecialAntibioticUsageRate(startTime, endTime);
            }
            catch (Exception)
            {

                viewModel = null;
            }
            return PartialView("_GetInPatientSpecialAntibioticUsageRate", viewModel);
        }
        /// <summary>
        /// 抗菌药物费用占药费总额的百分率
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult InPatientAntibioticUsageRateIndex(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new InPatientAntibioticUsageRate();
            try
            {
                viewModel = this.factory.CreateInPatientAntibioticUsageRate().GetInPatientAntibioticUsageRate(startTime, endTime);
            }
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                viewModel = new InPatientAntibioticUsageRate();
            }
            return PartialView("_GetInPatientAntibioticUsageRate", viewModel);
        }

        /// <summary>
        /// 住院患者人均使用抗菌药物费用
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult InPatientAverageAntibioticCostRateIndex(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new InPatientAverageAntibioticCostRate();
            try
            {
                viewModel = this.factory.CreateInPatientAverageAntibioticCostRate().GetInPatientAverageAntibioticCostRate(startTime, endTime);
            }
            catch (Exception)
            {

                viewModel = null;
            }
            return PartialView("_GetInPatientAverageAntibioticCostRate", viewModel);
        }
        #endregion
        #region 药物品种数
        /// <summary>
        /// 出院患者使用抗菌药物总品种数,出院患者使用药物总费用,出院患者使用抗菌药物费用
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public ActionResult InPatientDrugMessageIndex(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new InPatientDrugMessage();
            try
            {
                viewModel = this.factory.CreateInPatientDrugMessage().GetInPatientDrugMessage(startTime, endTime);
            }
            catch (Exception)
            {

                viewModel = new InPatientDrugMessage
                {
                    AntibioticCategoryNumber = -1,
                    AntibioticCost = -1,
                    TotalDrugCost = -1,
                    UnionAntibioticPerson = -1
                };
            }
            return PartialView("_GetInPatientDrugMessage", viewModel);
        }
        /// <summary>
        /// 住院患者人均使用抗菌药物品种数
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult InPatientAverageAnbitioticCategoryRateIndex(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new InPatientAverageAntibioticCategoryRate();
            try
            {
                viewModel = this.factory.CreateInPatientAverageAntibioticCategoryRate().GetInPatientAverageAntibioticCategoryRate(startTime, endTime);
            }
            catch (Exception)
            {

                viewModel = new InPatientAverageAntibioticCategoryRate();
            }
            return PartialView("_GetInPatientAverageAntibioticCategoryRate", viewModel);
        }
        #endregion
        #region 基本药物        
        /// <summary>
        /// 获取科室基本药物费用使用率
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult InPatientDepartmentEssentialUsageRateIndex(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new DepartmentEssentialUsageRate();
            try
            {
                viewModel = this.factory.CreateDepartmentEssentialUsageRate().GetDepartmentEssentialUsageRate(startTime, endTime);
            }
            catch (InvalidOperationException)
            {

                viewModel = null;
            }
            return PartialView("_GetDepartmentEssentialUsageRate", viewModel);
        }
        #endregion
    }
}