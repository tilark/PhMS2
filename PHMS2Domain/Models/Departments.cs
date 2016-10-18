namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Departments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Departments()
        {
            OutPatientPrescriptions = new HashSet<OutPatientPrescriptions>();
        }

        [Key]
        public int DepartmentId { get; set; }

        public int DEPT_ID_Origin { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        public int ParentDepartmentId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientPrescriptions> OutPatientPrescriptions { get; set; }
    }
}
