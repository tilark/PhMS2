using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhMS2dot1Domain.Models
{
    [Table("InPatients")]
    public class InPatient
    {
        public InPatient()
        {
            InPatientDrugRecords = new HashSet<InPatientDrugRecord>();
        }
        [Key]
        [Display(Name = "住院病历ID")]
        public virtual Guid InPatientID { get; set; }

        [Display(Name = "原HIS住院病历ID")]
        public virtual Guid? Origin_INPATIENT_ID { get; set; }

        [Display(Name = "病人信息ID")]
        public virtual Guid PatientID { get; set; }

        [Display(Name = "住院号")]
        public virtual String CaseNumber { get; set; }

        [Display(Name = "住院次数")]
        public virtual int Times { get; set; }

        [Display(Name = "入院时间")]
        public DateTime InDate { get; set; }
        [Display(Name = "入院科室（原HIS）")]

        public virtual long Origin_IN_DEPT { get; set; }
        [Display(Name = "出院时间")]
        public DateTime? OutDate { get; set; }

        [Display(Name = "当前科室（原HIS）")]
        public virtual long Origin_DEPT_ID { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual ICollection<InPatientDrugRecord> InPatientDrugRecords { get; set; }
    }
}
