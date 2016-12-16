using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace PhMS2dot1Domain.Models
{
    [Table("Patients")]
    public class Patient
    {
        public Patient()
        {

        }
        [Key]
        [Display(Name = "病人信息ID")]
        public virtual Guid PatientID { get; set; }
        [Display(Name = "原HIS病人信息ID")]
        public virtual Guid? Origin_PATIENT_ID { get; set; }

        [Display(Name = "病人姓名")]
        public virtual string PatientName { get; set; }

        [Display(Name = "出生日期")]
        public virtual DateTime? BirthDate { get; set; }
        public virtual ICollection<InPatient> InPatients { get; set; }

        public virtual ICollection<OutPatient> OutPatients { get; set; }
    }
    
    public class PatientComparer : IEqualityComparer<Patient>
    {
        public bool Equals(Patient x, Patient y)
        {
            if (x != null && y != null & x.PatientID == y.PatientID)
                return true;
            else
                return false;
        }

        public int GetHashCode(Patient obj)
        {
            return obj.PatientID.GetHashCode();
        }
    }
}
