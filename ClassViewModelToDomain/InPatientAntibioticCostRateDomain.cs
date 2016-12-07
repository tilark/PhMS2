using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain
{
    public class InPatientAntibioticCostRateDomain
    {
        private Decimal totalAntibioticCost;
        private Decimal totalAllDrugCost;
        public virtual Decimal TotalAntibioticCost
        {
            get { return Decimal.Round(this.totalAntibioticCost, 2); }
            set { this.totalAntibioticCost = value; }
        }
        public virtual Decimal TotalDrugCost
        {
            get { return Decimal.Round(this.totalAllDrugCost, 2); }
            set { this.totalAllDrugCost = value; }
        }
    }
}
