using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2.Models.ViewModels.InPatientReporter
{

    public class SpecialAntibioticUsageRate
    {
        public SpecialAntibioticUsageRate()
        {
            this.SpecialAntibioticDdds = 0;
            this.TotalAntibioticDdds = 0;
        }
        public decimal SpecialAntibioticDdds { get; set; }
        public Decimal TotalAntibioticDdds { get; set; }

        public Decimal UsageRate
        {
            get
            {
                return this.TotalAntibioticDdds != 0 ? Decimal.Round(this.SpecialAntibioticDdds * 100 / this.TotalAntibioticDdds, 2)
                    : 0;
            }
        }
    }
}
