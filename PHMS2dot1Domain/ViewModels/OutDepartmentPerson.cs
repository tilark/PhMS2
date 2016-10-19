using System;
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
        public virtual int DepartmentID { get; set; }
        public virtual int InPatientNumber { get; set; }
    }
}
