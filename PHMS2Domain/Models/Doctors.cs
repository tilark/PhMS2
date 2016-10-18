namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Doctors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctors()
        {
            OutPatientPrescriptions = new HashSet<OutPatientPrescriptions>();
        }

        [Key]
        public int DoctorId { get; set; }

        public int EMPLOYEE_ID_Origin { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        [StringLength(20)]
        public string DoctorNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientPrescriptions> OutPatientPrescriptions { get; set; }
    }
}
