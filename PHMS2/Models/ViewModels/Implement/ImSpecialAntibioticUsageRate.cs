using PHMS2.Models.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PHMS2.Models.ViewModels.InPatientReporter;
using ClassViewModelToDomain.IFactory;

namespace PHMS2.Models.ViewModels.Implement
{
    public class ImSpecialAntibioticUsageRate : ISpecialAntibioticUsageRate
    {
        private readonly IDomainFacotry DomainFactory;

        public ImSpecialAntibioticUsageRate(IDomainFacotry factory)
        {
            this.DomainFactory = factory;
        }
        public SpecialAntibioticUsageRate GetSpecialAntibioticUsageRate(DateTime startTime, DateTime endTime)
        {
            var result = new SpecialAntibioticUsageRate();
            try
            {
                result = new SpecialAntibioticUsageRate
                {
                    SpecialAntibioticDdds = this.DomainFactory.CreateSpecialAntibioticDdds().GetSpecialAntibioticDdds(startTime, endTime),
                   TotalAntibioticDdds = this.DomainFactory.CreateTotalAntibioticDdds().GetTotalAntibioticDdds(startTime, endTime)
                };
            }
            catch(Exception e)
            {
                throw new InvalidOperationException(String.Format("数据库读取错误!{0}", e.Message));
            }
            return result;
        }
    }
}