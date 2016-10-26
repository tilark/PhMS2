using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImInPatientDrugMessage : IInPatientDrugMessage
    {
        private readonly IDomainFacotry DomainFactory;

        public ImInPatientDrugMessage(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public InPatientDrugMessage GetInPatientDrugMessage(DateTime startTime, DateTime endTime)
        {
            var result = new InPatientDrugMessage();
            try
            {
                result = new InPatientDrugMessage
                {
                    AntibioticCategoryNumber = this.DomainFactory.CreateInPatientAntibioticCategoryNumber().GetAntibioticCategoryNumber(startTime, endTime),
                    AntibioticCost = this.DomainFactory.CreateInPatientAntibioticCost().GetInPatientAntibioticCost(startTime, endTime),
                    TotalDrugCost = this.DomainFactory.CreateInPatientDrugCost().GetPatientCost(startTime, endTime),
                    UnionAntibioticPerson = this.DomainFactory.CreateUnionAntibioticPerson().GetUnionAntibioticPerson(startTime, endTime)
                };
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("读取数据库出错! {0}", e.Message));
            }
            return result;
        }
    }
}