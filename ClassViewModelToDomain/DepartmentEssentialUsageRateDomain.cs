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
            this.essentialCost = Decimal.Zero;
            this.TotalDrugCost = Decimal.Zero;
        }
        private Decimal essentialCost;
        private Decimal totalDrugCost;
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Decimal EssentialCost
        {
            get { return Decimal.Round(this.essentialCost, 2); }
            set { this.essentialCost = value; }
        }
        //public int CancelAntibioticPerson { get; set; }
        public Decimal TotalDrugCost
        {
            get { return Decimal.Round(this.totalDrugCost, 2); }
            set { this.totalDrugCost = value; }
        }
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
