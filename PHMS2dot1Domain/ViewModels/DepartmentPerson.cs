using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class DepartmentPerson
    {
        public virtual int DepartmentID { get; set; }
        public virtual Decimal preStartTimeCost { get; set; }
        public virtual Decimal preEndTimeCost { get; set; }

        public virtual Decimal Cost { get; set; }
        public virtual int PersonPositive
        {
            get
            {
                return (preStartTimeCost == 0 && preEndTimeCost > 0) ? 1 : 0;
            }
        }

        public virtual int PersonNegative
        {
            get
            {
                return (preStartTimeCost > 0 && preEndTimeCost == 0) ? -1 : 0;
            }
        }
        public virtual int PersonNumber
        {
            get
            {
                if (preStartTimeCost > 0 && preEndTimeCost == 0)
                {
                    return -1;
                }
                else if (preStartTimeCost == 0 && preEndTimeCost > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
