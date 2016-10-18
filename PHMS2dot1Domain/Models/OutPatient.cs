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
    [Table("OutPatients")]

    public class OutPatient
    {
        public OutPatient()
        {
            OutPatientPrescriptions = new HashSet<OutPatientPrescription>();
        }
        [Key]
        [Display(Name = "门诊病例ID")]
        public virtual Guid OutPatientID { get; set; }

        [Display(Name = "原HIS门诊挂号ID")]
        public virtual Guid? Origin_GHXXID { get; set; }

        [Display(Name = "挂号类别")]
        public virtual int? Origin_GHLB { get; set; }

        [Display(Name = "挂号时间")]
        public virtual DateTime ChargeTime { get; set; }

        [Display(Name = "取消挂号时间")]
        public virtual DateTime CancelChargeTime { get; set; }

        [Display(Name = "病人信息ID")]
        public virtual Guid PatientID { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual HashSet<OutPatientPrescription> OutPatientPrescriptions { get; set; }
    }
}
