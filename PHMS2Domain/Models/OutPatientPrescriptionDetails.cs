namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutPatientPrescriptionDetails
    {
        public Guid OutPatientPrescriptionDetailsId { get; set; }

        public Guid OutPatientPrescriptionId { get; set; }

        public Guid CFMXID_Origin { get; set; }

        public int DrugMaintenanceId { get; set; }
        [ForeignKey("UndoOutPatientPrescriptionDetail")]

        public Guid? UndoOutPatientPrescriptionDetailsID { get; set; }
        [ForeignKey("Usages")]
        public Guid UsageID { get; set; }


        [Column(TypeName = "numeric")]
        public decimal Quantity { get; set; }

        public virtual DrugMaintenances DrugMaintenances { get; set; }

        public virtual OutPatientPrescriptions OutPatientPrescriptions { get; set; }

        public virtual OutPatientPrescriptionDetails UndoOutPatientPrescriptionDetail { get; set; }

        public virtual Usages Usages { get; set; }
    }
}
