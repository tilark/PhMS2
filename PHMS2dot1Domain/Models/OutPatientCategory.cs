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
    [Table("OutPatientCategories")]

    public class OutPatientCategory
    {
        [Display(Name = "门诊挂号类别ID")]
        public virtual int OutPatientCategoryID { get; set; }

        [Display(Name = "原HIS的挂号类别")]
        public virtual int? Origin_GHLB { get; set; }

        [Display(Name = "门诊挂号类别名称")]
        public virtual string OutPatientCategoryName { get; set; }

        [Display(Name = "是否门诊")]
        public virtual bool IsClinic { get; set; }

        [Display(Name = "是否急诊")]
        public virtual bool IsEmergency { get; set; }

    }
}
