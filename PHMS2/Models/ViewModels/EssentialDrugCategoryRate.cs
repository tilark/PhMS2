using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModel
{
    public class EssentialDrugCategoryRate
    {       
        public EssentialDrugCategoryRate()
        {
            this.EssentialDrugNums = 0;
            this.DrugCategoriesNums = 0;
        }
        public int EssentialDrugNums { get; set; }
        public int DrugCategoriesNums { get; set; }
        public Decimal Rate {
            get {
                return this.DrugCategoriesNums != 0 
                    ? Decimal.Round( (Decimal)this.EssentialDrugNums * 100 / (Decimal)this.DrugCategoriesNums , 2)
                   : 0;
            } }
    }
}