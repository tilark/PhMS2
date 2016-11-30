using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhMS2dot1Domain.ViewModels
{
    public class InPatientView
    {
        [Display(Name = "住院病历ID")]
        public virtual Guid InPatientID { get; set; }
        [Display(Name = "住院号")]
        public virtual String CaseNumber { get; set; }

        [Display(Name = "入院时间")]
        public DateTime InDate { get; set; }
        [Display(Name = "入院科室（原HIS）")]

        public virtual long Origin_IN_DEPT { get; set; }
        [Display(Name = "出院时间")]
        public DateTime? OutDate { get; set; }

        [Display(Name = "当前科室（原HIS）")]
        public virtual long Origin_DEPT_ID { get; set; }

        public List<InPatientDrugRecordView> InPatientDrugRecordViews { get; set; }



        public AntibioticPerson AntibioticDepartmentPerson(DateTime startTime, DateTime endTime)
        {
            var result = new AntibioticPerson();
            decimal preStartTimeCost = 0M;
            decimal preEndTimeCost = 0M;
            result.DepartmentID = (int)this.Origin_DEPT_ID;
            if (this.OutDate.HasValue && this.OutDate.Value >= startTime)
            {
                //outDate在取定时间段内的情况
                preEndTimeCost = this.InPatientDrugRecordViews.Sum(a => a.AntibioticCost(this.InDate, endTime));

            }
            else if (this.OutDate.HasValue && this.OutDate.Value < startTime)
            {
                //outDate在startTime之间的情况
                preStartTimeCost = this.InPatientDrugRecordViews.Sum(g => g.AntibioticCost(this.InDate, startTime));
                preEndTimeCost = this.InPatientDrugRecordViews.Sum(g => g.AntibioticCost(this.InDate, endTime));
            }
            if (preStartTimeCost > 0 && preEndTimeCost == 0)
            {
                result.AntibioticPatientNumber = -1;
            }
            else if (preStartTimeCost == 0 && preEndTimeCost > 0)
            {
                result.AntibioticPatientNumber = 1;
            }
            else
            {
                result.AntibioticPatientNumber = 0;
            }
            return result;
        }
        public Decimal AntibioticCost(DateTime startTime, DateTime endTime)
        {
            Decimal result = 0;
            result = this.InPatientDrugRecordViews.Sum(i => i.AntibioticCost(startTime, endTime));
            return result;
        }
    }
}
