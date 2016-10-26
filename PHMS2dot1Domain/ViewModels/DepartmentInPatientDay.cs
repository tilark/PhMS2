using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class DepartmentInPatientDay
    {
        public virtual int DepartmentID { get; set; }
        public virtual int InPatientDay { get; set; }
    }
}
