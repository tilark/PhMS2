using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Implement
{
    public class ImInpatientEnssentialDrugRecordFees : IInPatientDrugRecordDrugFeeView
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInpatientEnssentialDrugRecordFees(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<InpatientDrugRecordDrugFeesView> GetInpatientDrugRecordFees(DateTime startTime, DateTime endTime)
        {
            var result = new List<InpatientDrugRecordDrugFeesView>();
            try
            {
                var all = (from a in this.context.InPatients
                          where a.OutDate.HasValue && !a.CaseNumber.Contains("XT")
                          join b in this.context.InPatientDrugRecords on a.InPatientID equals b.InPatientID
                          where b.IsEssential == true
                          join c in this.context.DrugFees on b.InPatientDrugRecordID equals c.InPatientDrugRecordID
                          where c.ChargeTime >= startTime && c.ChargeTime < endTime
                          select new InpatientDrugRecordDrugFeesView {  DepartmentID = a.Origin_DEPT_ID,  ActualPrice = c.ActualPrice }).ToList();
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
                                    select new InpatientDrugRecordDrugFeesView { DepartmentID = a.Origin_DEPT_ID,  ActualPrice = c.ActualPrice }).ToList();
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
