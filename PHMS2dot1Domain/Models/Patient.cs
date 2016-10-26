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
    [Table("Patients")]
    public class Patient
    {
        public Patient()
        {
            InPatients = new HashSet<InPatient>();
            OutPatients = new HashSet<OutPatient>();
        }
        [Key]
        [Display(Name = "病人信息ID")]
        public virtual Guid PatientID { get; set; }
        [Display(Name = "原HIS病人信息ID")]
        public virtual Guid? Origin_PATIENT_ID { get; set; }
        [Display(Name = "出生日期")]
        public virtual DateTime? BirthDate { get; set; }
        public virtual ICollection<InPatient> InPatients { get; set; }

        public virtual ICollection<OutPatient> OutPatients { get; set; }
    }
}
