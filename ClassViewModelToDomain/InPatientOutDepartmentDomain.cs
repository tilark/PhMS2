using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain
{
    public class InPatientOutDepartmentDomain
    {
        //出院科室
        public virtual int DepartmentID { get; set; }
        public virtual string DepartmentName { get; set; }

        public string CaseNumber { get; set; }
        public int Times { get; set; }
        //出院人数
        public virtual int RegisterPerson { get; set; }
    }
}
