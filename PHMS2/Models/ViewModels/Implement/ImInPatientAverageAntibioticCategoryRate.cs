using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImInPatientAverageAntibioticCategoryRate : IInPatientAverageAntibioticCategoryRate
    {
        private readonly IDomainFacotry DomainFactory;

        public ImInPatientAverageAntibioticCategoryRate(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public InPatientAverageAntibioticCategoryRate GetInPatientAverageAntibioticCategoryRate(DateTime startTime, DateTime endTime)
        {
            var result = new InPatientAverageAntibioticCategoryRate();
            try
            {
                result = new InPatientAverageAntibioticCategoryRate
                {
                    TotalAntibioticCategoryNumber = this.DomainFactory.CreateInPatientAntibioticCategoryNumber().GetAntibioticCategoryNumber(startTime, endTime),
                    TotalAntibioticPerson = this.DomainFactory.CreateInPatientAntibioticPerson().GetAntibioticPerson(startTime, endTime)
                };
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据库读取错误！ {0}", e.Message));
            }
            return result;
        }
    }
}