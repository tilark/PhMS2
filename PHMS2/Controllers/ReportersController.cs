using PHMS2.Controllers.UnitOfWork;
using PHMS2.Models.ViewModel;
using PHMS2.Models.ViewModel.Interface;
using PHMS2Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassViewModelToDomain;
using PHMS2.Models.Factories;

namespace PHMS2.Controllers
{
    public class ReportersController : Controller
    {
        //private ReporterUnitOfWork unitOfWork = null;
        private readonly IReporterViewFactory ReporterViewFactory;
        public ReportersController(IReporterViewFactory factory)
        {
            this.ReporterViewFactory = factory;
        }
        // GET: Reporters
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportsList()
        {
            return View();
        }

        #region 抗菌药物
        /// <summary>
        ///使用金额排名前10名抗菌药物对应每一种药品使用金额前三位医生
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetTopTenAntibiotic(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            List<DrugTopRank> viewModel = new List<DrugTopRank>();
            try
            {
                var drugTopRank = this.ReporterViewFactory.CreateDrugTopRank(EnumDrugCategory.ANTIBIOTIC_DRUG);
                viewModel = drugTopRank.GetDrugTopRankList(startTime, endTime);
            }
            catch (Exception)
            {
                viewModel = null;
            }            
            return PartialView("_GetTopTenAntibiotic", viewModel);
        }
        /// <summary>
        /// 使用金额排名前10名抗菌药物对应每一种药品使用金额前三位科室
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public ActionResult GetTopTenAntibioticDepartment(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            List<DrugTopRank> viewModel = new List<DrugTopRank>();
            try
            {
                var drugTopRank = this.ReporterViewFactory.CreateDrugTopRank(EnumDrugCategory.ANTIBIOTIC_DRUG_DEP);
                viewModel = drugTopRank.GetDrugTopRankList(startTime, endTime);
            }
            catch (Exception)
            {
                viewModel = null;
            }

            return PartialView("_GetTopTenAntibioticDepartment", viewModel);
        }
        /// <summary>
        /// 获取门诊抗病菌使用率
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetOutpatientAntimicrobialRate(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            //单元测试时暂时删除，需恢复
            //endTime = endTime.AddDays(1);
            AntibioticUsageRate viewModel = null;
            try
            {
                IAntibioticUsageRate iAntibioticRate = this.ReporterViewFactory.CreateAntibioticUsageRate();
                viewModel = iAntibioticRate.GetAntibioticUsageRate(startTime, endTime, EnumOutPatientCategories.OUTPATIENT);
            }
            catch (Exception)
            {
                viewModel = null;
            }
            finally
            {
                if (viewModel == null)
                {
                    viewModel = new AntibioticUsageRate
                    {
                        AntibioticPerson = 0,
                        RegisterPerson = 0,
                    };
                }
            }
            //return View(viewModel);
            return PartialView("_GetOutpatientAntibioticRate", viewModel);
        }

        /// <summary>
        /// 获取急诊抗病菌使用率
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetEmergyAntimicrobialRate(DateTime startTime, DateTime endTime)
        {
            AntibioticUsageRate viewModel = null;
            try
            {
                ViewBag.startTime = startTime;
                ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
                endTime = endTime.AddDays(1);
                IAntibioticUsageRate iAntibioticRate = this.ReporterViewFactory.CreateAntibioticUsageRate();
                viewModel = iAntibioticRate.GetAntibioticUsageRate(startTime, endTime, EnumOutPatientCategories.EMERGEMENT);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (viewModel == null)
                {
                    viewModel = new AntibioticUsageRate
                    {
                        AntibioticPerson = 0,
                        RegisterPerson = 0,
                    };
                }
            }
            return PartialView("_GetEmergencyAntibioticRate", viewModel);
        }
        #endregion
        #region 药物品种
        /// <summary>
        /// 药物使用金额排前30个品种及每个品种排前3名的医生
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        /// 
        public ActionResult GetTopThirtyDrug(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            List<DrugTopRank> viewModel = null;
            try
            {
                var drugTopRank = this.ReporterViewFactory.CreateDrugTopRank(EnumDrugCategory.ALL_DRUG);
                viewModel = drugTopRank.GetDrugTopRankList(startTime, endTime);
            }
            catch (Exception)
            {
                viewModel = null;
            }
            return PartialView("_GetTopThirtyDrug", viewModel);
        }
        /// <summary>
        /// 每次就诊人均用药品种数
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetAverageDrugCategory(DateTime startTime, DateTime endTime)
        {
            return PartialView("_GetAverageDrugCategory");
        }
        #endregion
        #region 药物费用
        /// <summary>
        /// 门诊药物平均费用.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetOutPatientAverageCostView(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);

            //var viewModel = GetOutPatientAverageCost(startTime, endTime);
            IPatientAverageCost iAverageCost = this.ReporterViewFactory.CreatePatientAverageCost(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT);
            var viewModel = iAverageCost.GetOutPatientAverageCost(startTime, endTime);
            return PartialView("_GetOutpatientAverageCost", viewModel);

        }
        #endregion
        #region 处方药物情况
        /// <summary>
        /// 门、急诊患者药物处方情况集合
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetOutPatientDrugDetails(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            IPrescriptionMessageCollection iPrescriptionMessage = this.ReporterViewFactory.CreatePrescriptionMessageCollection();
            var viewModel = iPrescriptionMessage.GetPrescriptionMessageCollection(startTime, endTime);
            return PartialView("_GetOutPatientDrugDetails", viewModel);

        }
        #region 基本药物
        /// <summary>
        /// 基本药物占处方用药的百分率
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetEssentialDrugRate(DateTime startTime, DateTime endTime)
        {
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime.AddDays(1).AddMilliseconds(-1);
            endTime = endTime.AddDays(1);
            EssentialDrugCategoryRate viewModel = null;
            var drugRateModel = this.ReporterViewFactory.CreateEssentialDrugRate();
            try
            {
                viewModel = drugRateModel.GetEssentialDrugCategoryRate(startTime, endTime);
            }
            catch (Exception)
            {
                viewModel = new EssentialDrugCategoryRate
                {
                    DrugCategoriesNums = -1,
                    EssentialDrugNums = -1                    
                };

            }
            return PartialView("_GetEssentialDrugRate", viewModel);

        }
        #endregion

        #endregion    
        #region  静脉输液
        /// <summary>
        /// 门诊患者静脉输液使用率
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetOutPatientVeinInfusionRate(DateTime startTime, DateTime endTime)
        {
            return PartialView("_GetOutPatientVeinInfusionRate");

        }
        /// <summary>
        /// 急诊患者静脉输液使用率.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetEmergyVeinInfusionRate(DateTime startTime, DateTime endTime)
        {
            return PartialView("_GetEmergencyVeinInfusionRate");

        }
        #endregion
    }
}