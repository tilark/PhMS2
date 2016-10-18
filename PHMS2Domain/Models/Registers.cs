namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Registers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Registers()
        {
            OutPatientPrescriptions = new HashSet<OutPatientPrescriptions>();
        }

        [Key]
        public Guid RegisterId { get; set; }

        public Guid GHXXID_Origin { get; set; }

        public DateTime ChargeTime { get; set; }

        public int RegisterCategoryId { get; set; }

        public DateTime? CancelRegisterTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientPrescriptions> OutPatientPrescriptions { get; set; }

        public virtual RegisterCategories RegisterCategories { get; set; }
    }
}
