using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using PhMS2dot1Domain.Models;
using PhMS2dot1Domain.ViewModels;
using System.IO;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatientDrugRecordDrugFeesView : IInPatientDrugRecordDrugFeesView
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatientDrugRecordDrugFeesView(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }

       

        public List<InpatientDrugRecordDrugFeesView> GetInpatientDrugRecordDrugFeesView(DateTime startTime, DateTime endTime, Expression<Func<InpatientDrugRecordDrugFeesView, bool>> predicate = null)
        {
            var result = new List<InpatientDrugRecordDrugFeesView>();
            try
            {
                var sw = new StreamWriter(@"e:\databaseInPatientAntibioticLog6.log");
                this.context.Database.Log = s => { sw.Write(s); };
                result = (from a in this.context.InPatients
                                         where a.OutDate.HasValue && !a.CaseNumber.Contains("XT")
                                         join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                                         join d in this.context.AntibioticLevels on b.Origin_KSSDJID equals d.Origin_KSSDJID
                                         join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                                         where (a.OutDate.Value >= startTime && a.OutDate.Value < endTime && c.ChargeTime < endTime) || (a.OutDate.Value < startTime && c.ChargeTime >= startTime && c.ChargeTime < endTime)
                          select new InpatientDrugRecordDrugFeesView { InPatientID = a.InPatientID, CaseNumber = a.CaseNumber, InDate = a.InDate, OutDate = a.OutDate, KSSDJ = b.Origin_KSSDJID, DepartmentID = b.Origin_DEPT_ID, Origin_CJID = b.Origin_CJID, EffectiveConstituentAmount = b.EffectiveConstituentAmount, IsEssential = b.IsEssential, ChargeTime = c.ChargeTime, ActualPrice = c.ActualPrice, Quantity = c.Quantity, DDD = b.DDD, IsAntibiotic = d.IsAntibiotic,  IsSpecial = d.IsSpecial }).ToList();
                //OutDate在取定时间开始之前，取出相应的InPatientID信息，再根据该InPatientID获得该患者的全部住院信息
                var outDatePreStartTimeList = (from a in result                                              
                                               where a.OutDate.Value < startTime 
                                               select new { InPatientID = a.InPatientID }).Distinct();


                if (outDatePreStartTimeList.Count() > 0)
                {
                    //数量很少，没优化处理
                    foreach (var item in outDatePreStartTimeList)
                    {
                        var temp = (from a in this.context.InPatients
                                    where a.InPatientID == item.InPatientID
                                    join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                                    join d in this.context.AntibioticLevels on b.Origin_KSSDJID equals d.Origin_KSSDJID
                                    join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                                    select new InpatientDrugRecordDrugFeesView { InPatientID = a.InPatientID, CaseNumber = a.CaseNumber, InDate = a.InDate, OutDate = a.OutDate, KSSDJ = b.Origin_KSSDJID, DepartmentID = b.Origin_DEPT_ID, Origin_CJID = b.Origin_CJID, EffectiveConstituentAmount = b.EffectiveConstituentAmount, IsEssential = b.IsEssential, ChargeTime = c.ChargeTime, ActualPrice = c.ActualPrice, Quantity = c.Quantity, DDD = b.DDD, IsAntibiotic = d.IsAntibiotic, IsSpecial = d.IsSpecial }).ToList();
                        result.AddRange(temp);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

    }

}
