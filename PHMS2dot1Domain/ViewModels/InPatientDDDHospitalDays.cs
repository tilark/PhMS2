using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class InPatientDDDHospitalDays
    {
        public Guid InPatientID { get; set; }
        public int DepartmentID { get; set; }
        public Decimal DDDs { get; set; }
        public int InHospitalDays { get; set; }
    }
}
