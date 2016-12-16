using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Models;
using System.IO;
namespace PhMS2dot1Domain.Implement
{
    public class ImInpatientAntibioticDrugRecordFees : IInPatientDrugRecordDrugFeeView
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInpatientAntibioticDrugRecordFees(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InpatientDrugRecordDrugFeesView> GetInpatientDrugRecordFees(DateTime startTime, DateTime endTime)
        {
            var result = new List<InpatientDrugRecordDrugFeesView>();
            try
            {
                //var sw = new StreamWriter(@"e:\databaseInPatientAntibioticLog.log") { AutoFlush = true };
                //this.context.Database.Log = s => { sw.Write(s); };
                var all = (from a in this.context.InPatients
                                         where a.OutDate.HasValue && !a.CaseNumber.StartsWith("XT")
                                         join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                                         join d in this.context.AntibioticLevels on b.Origin_KSSDJID equals d.Origin_KSSDJID
                                         where d.IsAntibiotic == true
                                         join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                                         where  c.ChargeTime >= startTime && c.ChargeTime < endTime
                                         select new InpatientDrugRecordDrugFeesView { InPatientID = a.InPatientID, InDate = a.InDate, OutDate = a.OutDate, KSSDJ = b.Origin_KSSDJID, DepartmentID = b.Origin_DEPT_ID, Origin_CJID = b.Origin_CJID, EffectiveConstituentAmount = b.EffectiveConstituentAmount, ChargeTime = c.ChargeTime, ActualPrice = c.ActualPrice, Quantity = c.Quantity, DDD = b.DDD }).ToList();
                result.AddRange(all.Where(a => startTime <= a.OutDate.Value));

                //OutDate在取定时间开始之前，取出相应的InPatientID信息，再根据该InPatientID获得该患者的全部住院信息
                var outDatePreStartTimeList = all.Where(a => a.OutDate.Value < startTime).ToList();

                if (outDatePreStartTimeList.Count() > 0)
                {
                    //数量很少，没优化处理
                    foreach (var item in outDatePreStartTimeList)
                    {
                        var temp = (from a in this.context.InPatients
                                    where a.InPatientID == item.InPatientID
                                    join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                                    join d in this.context.AntibioticLevels on b.Origin_KSSDJID equals d.Origin_KSSDJID
                                    where d.IsAntibiotic
                                    join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                                    select new InpatientDrugRecordDrugFeesView { InPatientID = a.InPatientID, DepartmentID = b.Origin_DEPT_ID, ActualPrice = c.ActualPrice, EffectiveConstituentAmount = b.EffectiveConstituentAmount, IsEssential = b.IsEssential, IsAntibiotic = d.IsAntibiotic }).ToList();
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
