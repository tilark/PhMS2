namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutPatientReceipts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OutPatientReceipts()
        {
            OutPatientPrescriptions = new HashSet<OutPatientPrescriptions>();
            OutPatientReceiptDetails = new HashSet<OutPatientReceiptDetails>();
        }

        [Key]
        public Guid OutPatientReceiptId { get; set; }

        public Guid FPID_Origin { get; set; }

        public DateTime ChargeTime { get; set; }

        public DateTime? RefundTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientPrescriptions> OutPatientPrescriptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientReceiptDetails> OutPatientReceiptDetails { get; set; }
    }
}
