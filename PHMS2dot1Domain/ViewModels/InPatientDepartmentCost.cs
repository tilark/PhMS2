﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class InPatientDepartmentCost
    {
        public Guid InPatientID { get; set; }
        public int DepartmentID { get; set; }
        public Decimal Cost { get; set; }

        [Display(Name = "原HIS药物CJID")]
        public virtual int Origin_CJID { get; set; }
        [Display(Name = "药物品名")]
        public virtual string ProductName { get; set; }
        public long Count { get; set; }


    }
}
