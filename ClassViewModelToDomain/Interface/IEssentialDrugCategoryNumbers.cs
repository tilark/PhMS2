using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain.Interface
{
    public interface IEssentialDrugCategoryNumbers
    {
        int GetEssentialDrugCategoryNumbers(DateTime startTime, DateTime endTime);
    }
}
