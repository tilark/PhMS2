using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImInPatientAntibioticUsageRate : IInPatientAntibioticUsageRate
    {
        private readonly IDomainFacotry factory;

        public ImInPatientAntibioticUsageRate(IDomainFacotry factory)
        {
            this.factory = factory;
        }
        public InPatientAntibioticUsageRate GetInPatientAntibioticUsageRate(DateTime startTime, DateTime endTime)
        {
            var result = new InPatientAntibioticUsageRate();
            try
            {
                result = new InPatientAntibioticUsageRate
                {
                    TotalAntibioticCost = this.factory.CreateInPatientAntibioticCost().GetInPatientAntibioticCost(startTime, endTime),
                    TotalDrugCost = this.factory.CreateInPatientDrugCost().GetPatientCost(startTime, endTime)
                };
            }
            catch (Exception e)
            {

                throw new ArgumentNullException("读取数据错误！{0}", e.Message);
            }
            return result;
        }
    }
}