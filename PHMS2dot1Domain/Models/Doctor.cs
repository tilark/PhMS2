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
    [Table("Doctors")]
    public class Doctor
    {
        [Key]
        [Display(Name = "医生ID")]
        public virtual int DoctorID { get; set; }
        [Display(Name = "原HIS医生ID")]
        public virtual int? ORIGIN_EMPLOYEE_ID { get; set; }
        [Display(Name = "医生姓名")]
        public virtual string DoctorName { get; set; }
        [Display(Name = "医生工号")]
        public virtual string DoctorCode { get; set; }
    }
}
