using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            this.context.Database.Log = Console.WriteLine;
            try
            {
                var inpatient = (from a in this.context.InPatients
                                where a.OutDate.HasValue && a.OutDate.Value >= startTime && a.OutDate.Value < endTime
                                select new { InPatient = a.InPatientID, InDate = a.InDate, OutDate = a.OutDate }).ToList();

                var inpatientDrugRecordFees = (from a in this.context.InPatients
                                               where a.OutDate.HasValue
                                               from b in this.context.InPatientDrugRecords

                                               from c in this.context.DrugFees
                                               where c.ChargeTime >= startTime && c.ChargeTime < endTime
                                               select new { InPatientID = a.InPatientID, InDate = a.InDate, OutDate = a.OutDate, KSSDJ = b.Origin_KSSDJ, DepartmentID = b.Origin_EXEC_DEPT, ChargeTime = c.ChargeTime, ActualPrice = c.ActualPrice, Quantity = c.Quantity }).ToList();
                //where b.Origin_KSSDJ >= 1 && b.Origin_KSSDJ <= 3                                      
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