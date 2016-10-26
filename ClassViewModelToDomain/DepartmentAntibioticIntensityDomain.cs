using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain
{
    public class DepartmentAntibioticIntensityDomain
    {
        public DepartmentAntibioticIntensityDomain()
        {
            this.AntibioticDdd = 0;
            this.PersonNumberDays = 0;
        }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        //住院抗菌药物消耗量（累计DDD数
        public Decimal AntibioticDdd { get; set; }
        //同期收治患者人天数：同期出院患者人数×同期出院患者平均住院天数
        public int PersonNumberDays { get; set; }

        public Decimal IntensityRate
        {
            get
            {
                return this.PersonNumberDays != 0
                   ? Decimal.Round((Decimal)this.AntibioticDdd * 100 / (Decimal)this.PersonNumberDays, 2)
                  : 0;
            }
        }
    }
}
