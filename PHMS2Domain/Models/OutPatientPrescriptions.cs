namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutPatientPrescriptions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OutPatientPrescriptions()
        {
            OutPatientPrescriptionDetails = new HashSet<OutPatientPrescriptionDetails>();
        }

        [Key]
        public Guid OutPatientPrescriptionId { get; set; }

        public Guid CFID_Origin { get; set; }

        public DateTime ChargeTime { get; set; }

        public bool IsValid { get; set; }

        public Guid RegisterId { get; set; }

        public Guid OutPatientReceiptId { get; set; }

        public int DoctorId { get; set; }

        public int DepartmentId { get; set; }

        public virtual Departments Departments { get; set; }

        public virtual Doctors Doctors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientPrescriptionDetails> OutPatientPrescriptionDetails { get; set; }

        public virtual OutPatientReceipts OutPatientReceipts { get; set; }

        public virtual Registers Registers { get; set; }
    }
}
