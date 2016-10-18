using System;
using System.Collections.Generic;


namespace ClassViewModelToDomain
{
    public class DrugTopRank
    {
       
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public Decimal Cost { get; set; }
        public bool IsAntibiotic { get; set; }
        public List<DrugDoctorDepartmentCost> DrugDoctorDepartmentCostList { get; set; }

    }
}