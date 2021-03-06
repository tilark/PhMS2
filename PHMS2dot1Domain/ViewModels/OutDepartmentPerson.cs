﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    /// <summary>
    /// 出院病人的入住科室及人数
    /// </summary>
    public class OutDepartmentPerson
    {
        //出院科室
        public virtual int DepartmentID { get; set; }

        //出院人数
        public virtual int InPatientNumber { get; set; }
    }
}
