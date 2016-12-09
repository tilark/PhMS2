using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class InpatientDrugRecordFees
    {
        public Guid InPatientID { get; set; }
        public DateTime InDate { get; set; }
        public DateTime? OutDate { get; set; }

        public int? KSSDJ { get; set; }
        public long DepartmentID { get; set; }
        public DateTime ChargeTime { get; set; }
        public Decimal ActualPrice { get; set; }

        public virtual int Origin_CJID { get; set; }

        public Decimal Quantity { get; set; }
        public Decimal DDD { get; set; }

        //[Display(Name = "有效成分含量")]
        public virtual Decimal EffectiveConstituentAmount { get; set; }
        public bool IsEssential { get; set; }
        public bool IsAntibiotic { get; set; }
        public int InHospitalDays { get { return this.OutDate.HasValue ? this.OutDate.Value.Subtract(this.InDate).Days : 0; } }

        public Decimal EffectiveDDD
        {
            get
            {
                return this.EffectiveConstituentAmount / this.DDD * this.Quantity;
            }
        }
    }
}
