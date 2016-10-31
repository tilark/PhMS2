using ClassViewModelToDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.Reporter
{
    public class DrugTopThirtyDescription
    {
        public List<DrugDoctorCost> DrugDoctorCostList { get; set; }

        public Decimal TotalDrugCost
        {
            get
            {
                return this.DrugDoctorCostList.Sum(a => a.Cost);
            }
        }

    }
}