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
    [Table("Departments")]
    public class Department
    {
        [Key]
        [Display(Name = "科室ID")]
        public virtual int DepartmentID { get; set; }
        [Display(Name = "原HIS科室ID")]
        public virtual int? Origin_DEPT_ID { get; set; }
        [Display(Name = "科室名称")]
        public virtual string DepartmentName { get; set; }

    }
}
