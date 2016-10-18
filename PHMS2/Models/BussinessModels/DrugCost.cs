using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.BussinessModels
{
    public class DrugCost
    {
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public Decimal Cost { get; set; }
        public bool IsAntibiotic { get; set; }

    }
}