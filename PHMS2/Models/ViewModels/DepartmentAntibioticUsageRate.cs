using ClassViewModelToDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModel
{
    public class DepartmentAntibioticUsageRate
    {
        public List<DepartmentAntibioticUsageRateDomain> DepartmentAntibioticUsageRateList;

        public string TotalHosipitoal { get { return "全院"; } }
        public int TotalAntibioticPerson
        {
            get
            {
                return DepartmentAntibioticUsageRateList.Sum(d => d.AntibioticPerson);
            }
        }
        public int TotalRegisterPerson
        {
            get
            {
                return DepartmentAntibioticUsageRateList.Sum(d => d.RegisterPerson);
            }
        }

        public Decimal UsageRate
        {
            get
            {
                return DepartmentAntibioticUsageRateList.Sum(d => d.UsageRate);
            }
        }
    }
}