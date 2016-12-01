using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PhMS2dot1Domain.Models;
namespace PHMS2.Controllers
{
    public class InPatientReporter2Controller : Controller
    {
        private PhMS2dot1DomainContext context = new PhMS2dot1DomainContext();
        // GET: InPatientReporter2
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 住院患者抗菌药物使用率（分院科两级指标）.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult InPatientDepartmentAntibioticUsageRate(DateTime startTime, DateTime endTime)
        {
            endTime = endTime.AddDays(1);
            var sw = new StreamWriter(@"E:\linqquerydaa2.log") { AutoFlush = true };
            this.context.Database.Log = s => { sw.Write(s); };
            try
            {
                var inpatient = (from a in this.context.InPatients
                                where a.OutDate.HasValue && a.OutDate.Value >= startTime && a.OutDate.Value < endTime && !a.CaseNumber.Contains("XT")
                                select new { InPatient = a.InPatientID, DepartmentID = a.Origin_DEPT_ID, InDate = a.InDate, OutDate = a.OutDate }).ToList();

                
                var inpatientAnbtioticDrugRecordFees = (
                                               from a in this.context.InPatients
                                               where a.OutDate.HasValue &&  a.OutDate.Value < endTime
                                               join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                                               join d in this.context.AntibioticLevels on b.Origin_KSSDJ equals d.Origin_KSSDJ
                                               where d.IsAntibiotic == true
                                               join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                                               where c.ChargeTime >= startTime && c.ChargeTime < endTime
                                               select new { InPatientID = a.InPatientID, InDate = a.InDate, OutDate = a.OutDate, KSSDJ = b.Origin_KSSDJ, DepartmentID = b.Origin_EXEC_DEPT, ChargeTime = c.ChargeTime, ActualPrice = c.ActualPrice, Quantity = c.Quantity }).ToList();

                var inDurationList = from a in inpatientAnbtioticDrugRecordFees
                                     where a.OutDate.Value >= startTime
                                     group a by new { a.InPatientID, a.DepartmentID } into b
                                     where b.Sum(c => c.ActualPrice) > 0
                                     select new { InPatientID = b.Key.InPatientID, DepartmentID = b.Key.DepartmentID, Count = 1 };

                var preStartTimeCostList = from a in inpatientAnbtioticDrugRecordFees
                                           where a.OutDate.Value < startTime && a.InDate < startTime
                                           group a by new { a.InPatientID, a.DepartmentID } into b                                                                            
                                           select new { InPatientID = b.Key.InPatientID, DepartmentID = b.Key.DepartmentID, PreStartTimeCost = b.Sum(c => c.ActualPrice) };
                var preEndTimeCostList = from a in inpatientAnbtioticDrugRecordFees
                                         where a.OutDate.Value < startTime && a.InDate < endTime
                                         group a by new { a.InPatientID, a.DepartmentID } into b
                                         select new { InPatientID = b.Key.InPatientID, DepartmentID = b.Key.DepartmentID, PreEndTimeCost = b.Sum(c => c.ActualPrice) };

                //var preStartTimeList = from a in preStartTimeCostList
                //                       where a.PreStartTimeCost == 0
                //                       join b in preEndTimeCostList on a.DepartmentID equals b.DepartmentID;


                //根据获得信息根据科室分组，计算出抗菌药物的费用，并计算出有效人数
                //出院时间在startTime 和endTime之内，直接计算出总价格，再计算出人数
                //var inDurationPerson = from a in inpatientDrugRecordFees
                //                       where a.OutDate.Value >= startTime
                //                       group a by a.DepartmentID into g
                //                       select new { DepartmentID = g.Key, AntibioticPrice = g.Sum(b => b.ActualPrice) };

                //出院时间在startTime之前，需先判断在startTime之前的费用，再计算出endTime之后的费用，再判断是为0，还是-1



                var OutPatientPersonNumber = from a in this.context.InPatients
                                             where a.OutDate.HasValue && a.OutDate.Value >= startTime && a.OutDate.Value < endTime
                                             group a by a.Origin_DEPT_ID into g
                                             select new { DepartmentID = g.Key, RegisterPerson = g.Count() };
            }
            catch (Exception)
            {

                throw;
            }
            

            return View();
                                          
        }
    }
}