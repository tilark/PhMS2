using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.Reporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImDrugTopThirtyDescription : IDrugTopThirtyDescription
    {
        private readonly IDomainFacotry factory;

        public ImDrugTopThirtyDescription(IDomainFacotry factory)
        {
            this.factory = factory;
        }
        public DrugTopThirtyDescription GetDrugTopThirtyDescription(DateTime startTime, DateTime endTime)
        {
            var result = new DrugTopThirtyDescription();

            try
            {
                result.DrugDoctorCostList = this.factory.CreateInPatientDrugDoctorCostList().GetDrugDoctorCostList(startTime, endTime);
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据库读取错误！ {0}", e.Message));
            }
            return result;
        }
    }
}