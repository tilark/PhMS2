using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain
{
    public class DepartmentAntibioticUsageRateDomain 
    {
        public DepartmentAntibioticUsageRateDomain()
        {
            this.AntibioticPerson = 0;
            this.RegisterPerson = 0;
        }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int AntibioticPerson { get; set; }
        //public int CancelAntibioticPerson { get; set; }
        public int RegisterPerson { get; set; }
        public decimal UsageRate
        {
            get
            {
                return this.RegisterPerson != 0
                    ? Decimal.Round((Decimal)this.AntibioticPerson * 100 / (Decimal)this.RegisterPerson, 2)
                   : 0;
            }
        }

    }
}
