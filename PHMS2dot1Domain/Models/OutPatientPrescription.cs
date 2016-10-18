using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace PhMS2dot1Domain.Models
{
    [Table("OutPatientPrescriptions")]
    public class OutPatientPrescription
    {
        public OutPatientPrescription()
        {
            OutPatientDrugRecords = new HashSet<OutPatientDrugRecord>();
        }
        [Display(Name = "门诊处方ID")]
        public virtual Guid OutPatientPrescriptionID { get; set; }

        [Display(Name = "原HIS门诊处方ID")]
        public virtual Guid Origin_CFID { get; set; }

        [Display(Name = "门诊病例ID")]
        public virtual Guid OutPatientID { get; set; }

        [Display(Name = "收费时间")]
        public virtual DateTime ChargeTime { get; set; }

        [Display(Name = "原HIS科室代码")]
        public virtual int? Origin_KSDM { get; set; }

        [Display(Name = "原HIS医生代码")]
        public virtual int? Origin_YSDM { get; set; }

        public virtual OutPatient OutPatient { get; set; }

        public virtual HashSet<OutPatientDrugRecord> OutPatientDrugRecords { get; set; }
    }
}
