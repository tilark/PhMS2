using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class InPatientDepartmentDrugName
    {

        public Guid InPatientID { get; set; }
        public DateTime InDate { get; set; }
        public DateTime? OutDate { get; set; }

        public long DepartmentID { get; set; }
        public DateTime ChargeTime { get; set; }
        public Decimal ActualPrice { get; set; }
        public Decimal Quantity { get; set; }

        [Display(Name = "原HIS药物CJID")]
        public virtual int Origin_CJID { get; set; }
        [Display(Name = "药物品名")]
        public virtual string ProductName { get; set; }
    }
}
