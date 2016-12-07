using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    /// <summary>
    /// 抗菌药物占总费用
    /// </summary>
    public class InPatientAntibioticCostRate
    {
        public InPatientAntibioticCostRate()
        {
            this.totalAntibioticCost = 0;
            this.totalAllDrugCost = 0;
        }
        private Decimal totalAntibioticCost;
        private Decimal totalAllDrugCost;
        public virtual Decimal TotalAntibioticCost
        {
            get { return Decimal.Round(this.totalAntibioticCost, 2); }
            set { this.totalAntibioticCost = value; }
        }
        public virtual Decimal TotalDrugCost {
            get { return Decimal.Round(this.totalAllDrugCost, 2); }
            set { this.totalAllDrugCost = value; }
        }

        public Decimal Rate
        {
            get
            {
                return this.TotalDrugCost != 0 ? Decimal.Round(this.TotalAntibioticCost * 100 / this.TotalDrugCost, 2) : 0;
            }
        }
    }
}