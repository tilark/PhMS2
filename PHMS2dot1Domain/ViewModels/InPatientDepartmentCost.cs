using System;
using System.Collections.Generic;
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

        public long Count { get; set; }
    }
}
