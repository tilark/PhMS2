using ClassViewModelToDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    public class DepartmentAntibioticUsageRate
    {
        public List<DepartmentAntibioticUsageRateDomain> DepartmentAntibioticUsageRateList;
        public string Hospital
        {
            get
            {
                return "全院";
            }
        }
        public int TotalAntibioticPerson
        {
            get
            {
                return this.DepartmentAntibioticUsageRateList.Sum(d => d.AntibioticPerson);
            }
        }
        public int TotalRegisterPerson
        {
            get
            {
                return this.DepartmentAntibioticUsageRateList.Sum(d => d.RegisterPerson);
            }
        }
        public decimal TotalUsageRate
        {
            get
            {
                return   this.TotalRegisterPerson != 0
                    ? Decimal.Round((Decimal)this.TotalAntibioticPerson * 100 / (Decimal)this.TotalRegisterPerson, 2)
                   : 0; 
            }
        }
    }
}