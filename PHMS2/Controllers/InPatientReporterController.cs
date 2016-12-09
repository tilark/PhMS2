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
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Controllers
{
    public class InPatientReporterController : Controller
    {
        private readonly IDomainFacotry DomainFactory;

        public InPatientReporterController(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
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
        public ActionResult InPatientDepartmentAntibioticUsageRate(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            var viewModel = new DepartmentAntibioticUsageRate();
            viewModel.DepartmentAntibioticUsageRateList = new List<ClassViewModelToDomain.DepartmentAntibioticUsageRateDomain>();
            try
            {
                viewModel.DepartmentAntibioticUsageRateList = this.DomainFactory.CreateDepartmentAntibioticUsageRateDomain().GetDepartmentAntibioticUsageRateDomain(startTime, endTime);
            }
            catch (Exception e)
            {
                var tempData = new DepartmentAntibioticUsageRateDomain
                {
                    AntibioticPerson = -1,
                    RegisterPerson = -1,
                    DepartmentID = -1,
                    DepartmentName = e.Message
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
                viewModel.DepartmentAntibioticIntensityList = this.DomainFactory.CreateDepartmentAntibioticIntensityDomain().GetDepartmentAntibioticIntensityDomain(startTime, endTime);
            }
            catch (Exception e)
            {
                var temp = new DepartmentAntibioticIntensityDomain
                {
                    DepartmentID = -1,
                    DepartmentName = e.Message,
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
                viewModel = new SpecialAntibioticUsageRate
                {
                    SpecialAntibioticDdds = this.DomainFactory.CreateSpecialAntibioticDdds().GetSpecialAntibioticDdds(startTime, endTime),
                    TotalAntibioticDdds = this.DomainFactory.CreateTotalAntibioticDdds().GetTotalAntibioticDdds(startTime, endTime)
                };
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
            var viewModel = new InPatientAntibioticCostRate();
            try
            {
                var temp = this.DomainFactory.CreateInPatientAntibioticCostRateDomain().GetInPatientAntibioticCostRateDomain(startTime, endTime);
                viewModel = new InPatientAntibioticCostRate
                {
                    TotalAntibioticCost = temp.TotalAntibioticCost,
                    TotalDrugCost = temp.TotalDrugCost
                };
            }
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                viewModel = new InPatientAntibioticCostRate();
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
                viewModel = new InPatientAverageAntibioticCostRate
                {
                    TotalAntibioticCost = this.DomainFactory.CreateInPatientAntibioticCost().GetInPatientAntibioticCost(startTime, endTime),
                    TotalAntibioticPerson = this.DomainFactory.CreateInPatientAntibioticPerson().GetAntibioticPerson(startTime, endTime)
                };
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
                viewModel = new InPatientDrugMessage
                {
                    AntibioticCategoryNumber = this.DomainFactory.CreateInPatientAntibioticCategoryNumber().GetAntibioticCategoryNumber(startTime, endTime),
                    AntibioticCost = this.DomainFactory.CreateInPatientAntibioticCost().GetInPatientAntibioticCost(startTime, endTime),
                    TotalDrugCost = this.DomainFactory.CreateInPatientDrugCost().GetPatientCost(startTime, endTime),
                    UnionAntibioticPerson = this.DomainFactory.CreateUnionAntibioticPerson().GetUnionAntibioticPerson(startTime, endTime)
                };
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
                viewModel = new InPatientAverageAntibioticCategoryRate
                {
                    TotalAntibioticCategoryNumber = this.DomainFactory.CreateInPatientAntibioticCategoryNumber().GetAntibioticCategoryNumber(startTime, endTime),
                    TotalAntibioticPerson = this.DomainFactory.CreateInPatientAntibioticPerson().GetAntibioticPerson(startTime, endTime)
                };
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
                viewModel.DepartmentEssentialUsageRateList = this.DomainFactory.CreateDepartmentEssentialUsageRateDomain().GetDepartmentEssentialUsageRateDomain(startTime, endTime);
            }
            catch (Exception e)
            {
                var temp = new DepartmentEssentialUsageRateDomain
                {
                    DepartmentID = -1,
                    DepartmentName = e.Message,
                    EssentialCost = -1,
                    TotalDrugCost = 0
                };
                viewModel.DepartmentEssentialUsageRateList.Add(temp);

            }
            return PartialView("_GetDepartmentEssentialUsageRate", viewModel);
        }
        #endregion
    }
}