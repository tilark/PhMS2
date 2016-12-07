using ClassViewModelToDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    public class DepartmentEssentialUsageRate
    {
        public List<DepartmentEssentialUsageRateDomain> DepartmentEssentialUsageRateList;
        public string Hospital
        {
            get
            {
                return "全院";
            }
        }
        public Decimal TotalEssentialCost
        {
            get
            {
                return this.DepartmentEssentialUsageRateList.Sum(d => d.EssentialCost);
            }
        }
        public Decimal AllTotalDrugCost
        {
            get
            {
                return this.DepartmentEssentialUsageRateList.Sum(d => d.TotalDrugCost);
            }
        }
        public Decimal TotalUsageRate
        {
            get
            {
                return this.AllTotalDrugCost != 0
                   ? Decimal.Round(this.TotalEssentialCost * 100 / this.AllTotalDrugCost, 2)
                  : 0;
            }
        }
    }
}