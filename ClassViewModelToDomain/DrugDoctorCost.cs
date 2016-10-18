using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain
{
     public class DrugDoctorCost
    {
        public string DoctorName { get; set; }

        public string DepartmentName { get; set; }
        public string ProductNumber { get; set; }
        public bool IsAntibiotic { get; set; }
        public decimal Cost { get; set; }
    }
}
