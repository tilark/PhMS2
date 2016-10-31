using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientDrugDoctorCostList : IInPatientDrugDoctorCostList
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientDrugDoctorCostList(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }

        public List<DrugDoctorCost> GetDrugDoctorCostList(DateTime startTime, DateTime endTime)
        {
            var result = new List<DrugDoctorCost>();
            try
            {

            }
            catch (Exception)
            {

                throw new InvalidOperationException(String.Format("数据库读取错误！ {0}", e.Message));
            }
            return result;
        }
    }
}
