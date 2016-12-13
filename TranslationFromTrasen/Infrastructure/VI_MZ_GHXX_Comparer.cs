using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrasenLib;

namespace TranslationFromTrasen.Infrastructure
{
    class VI_MZ_GHXX_Comparer : IEqualityComparer<TrasenLib.VI_MZ_GHXX>
    {
        public bool Equals(VI_MZ_GHXX x, VI_MZ_GHXX y)
        {
            if (x != null && y != null && x.BRXXID == y.BRXXID)
                return true;
            else
                return false;
        }

        public int GetHashCode(VI_MZ_GHXX obj)
        {
            return obj.BRXXID.GetHashCode();
        }
    }
}
