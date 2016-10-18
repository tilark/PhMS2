using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PHMS2Domain.Models
{
    internal class EssentialDrugMessage : AbDrugMessage
    {
        public bool IsEssential { get { return true;  } }

    }
}
