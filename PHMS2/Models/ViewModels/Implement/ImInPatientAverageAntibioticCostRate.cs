using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImInPatientAverageAntibioticCostRate : IInPatientAverageAntibioticCostRate
    {
        private readonly IDomainFacotry factory;

        public ImInPatientAverageAntibioticCostRate(IDomainFacotry factory)
        {
            this.factory = factory;
        }
        public InPatientAverageAntibioticCostRate GetInPatientAverageAntibioticCostRate(DateTime startTime, DateTime endTime)
        {
            var result = new InPatientAverageAntibioticCostRate();
            try
            {
                result = new InPatientAverageAntibioticCostRate
                {
                    TotalAntibioticCost = this.factory.CreateInPatientAntibioticCost().GetInPatientAntibioticCost(startTime, endTime),
                    TotalAntibioticPerson = this.factory.CreateInPatientAntibioticPerson().GetAntibioticPerson(startTime, endTime)
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