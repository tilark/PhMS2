using PhMS2dot1Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Interface
{
    /// <summary>
    /// 获得住院患者、用药记录、用药费用的视图
    /// </summary>
    public interface IInPatientDrugRecordDrugFeesView
    {
        List<InpatientDrugRecordDrugFeesView> GetInpatientDrugRecordDrugFeesView(DateTime startTime, DateTime endTime, Expression<Func<InpatientDrugRecordDrugFeesView, bool>> predicate = null);

    }
}
