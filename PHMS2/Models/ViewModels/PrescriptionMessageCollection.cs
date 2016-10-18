using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassViewModelToDomain;

namespace PHMS2.Models.ViewModel
{
    public class PrescriptionMessageCollection
    {
        public PrescriptionMessage OutPatientMessage { get; set; }
        public PrescriptionMessage EmergencyMessage { get; set; }
        public PrescriptionMessage TotalMessage { get; set; }


    }
}