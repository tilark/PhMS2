using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain
{
    public class DepartmentEssentialUsageRateDomain
    {
        public DepartmentEssentialUsageRateDomain()
        {
            this.EssentialCost = Decimal.Zero;
            this.TotalDrugCost = Decimal.Zero;
        }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Decimal EssentialCost { get; set; }
        //public int CancelAntibioticPerson { get; set; }
        public Decimal TotalDrugCost { get; set; }
        public decimal UsageRate
        {
            get
            {
                return this.TotalDrugCost != 0
                    ? Decimal.Round(this.EssentialCost * 100 / this.TotalDrugCost, 2)
                   : 0;
            }
        }
    }
}
