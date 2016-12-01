using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class DepartmentInPatientDay
    {
        /// <summary>
        /// 科室及科室同期收治患者人天数.
        /// </summary>
        /// <value>The department identifier.</value>
        public virtual int DepartmentID { get; set; }
        public virtual int InPatientDay { get; set; }
    }
}
