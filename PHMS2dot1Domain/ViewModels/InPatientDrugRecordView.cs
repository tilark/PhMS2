using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhMS2dot1Domain.ViewModels
{
    public class InPatientDrugRecordView
    {
        [Display(Name = "药物记录ID")]
        public virtual Guid InPatientDrugRecordID { get; set; }
        [Display(Name = "原HIS抗菌药物等级")]
        public virtual int? Origin_KSSDJ { get; set; }
        [Display(Name = "原HIS药物CJID")]
        public virtual int Origin_CJID { get; set; }
        [Display(Name = "DDD值")]
        public virtual Decimal DDD { get; set; }

        public List<DrugFeeView> DrugFeeViews { get; set; }


        #region 抗菌药物

        public bool IsAntibioticDrug
        {
            get
            {
                return this.Origin_KSSDJ.HasValue && this.Origin_KSSDJ.Value >= 1 && this.Origin_KSSDJ.Value <= 3;
            }
        }

        public bool IsSpecialAntibioticDrug
        {
            get
            {
                return this.Origin_KSSDJ.HasValue && this.Origin_KSSDJ.Value == 3;
            }
        }
        public Decimal AntibioticCost(DateTime startTime, DateTime endTime)
        {
            return this.IsAntibioticDrug
                ? this.DrugFeeViews.Sum(d => d.ActualPriceInDuration(startTime, endTime))
                : 0;
        }
       
        #endregion
    }
}
