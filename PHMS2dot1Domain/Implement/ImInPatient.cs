﻿using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.Implement
{
    public class ImInPatient : IInPatient
    {
        private readonly PhMS2dot1DomainContext context;

        public ImInPatient(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public InPatient GetInPatient(Guid inPatientID)
        {
            InPatient result = null;
            int tryTimes = 0;
            do
            {
                tryTimes++;
                try
                {
                    //.Include("InPatientDrugRecords.DrugFees")
                    result = this.context.InPatients.Where(i => i.InPatientID == inPatientID).FirstOrDefault();
                    //getData = false;
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException)
                {
                    //getData = false;
                }
                catch (System.Data.Entity.Core.EntityException)
                {

                }
                catch (Exception)
                {
                    //getData = false;

                    //throw;
                }

            } while (tryTimes < 51);


            return result;
        }
    }
}
