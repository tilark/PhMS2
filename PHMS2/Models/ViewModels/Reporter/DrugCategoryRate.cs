using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.Reporter
{
    /// <summary>
    /// DrugCategoryRate，药物总品种数.
    /// </summary>
    public class DrugCategoryRate
    {
        public DrugCategoryRate()
        {
            this.DrugCategoryNums = 0;
            this.RegisterPersons = 0;
        }
        public int DrugCategoryNums { get; set; }
        public int RegisterPersons { get; set; }
        public Decimal Rate
        {
            get
            {
                return this.RegisterPersons != 0
                    ? Decimal.Round((Decimal)this.DrugCategoryNums * 100 / (Decimal)this.RegisterPersons, 2)
                   : 0;
            }
        }
    }
}