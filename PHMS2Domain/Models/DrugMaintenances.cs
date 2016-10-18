namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DrugMaintenances
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DrugMaintenances()
        {
            OutPatientPrescriptionDetails = new HashSet<OutPatientPrescriptionDetails>();
        }

        [Key]
        public int DrugMaintenanceId { get; set; }

        [Required]
        public string ProductNumber { get; set; }

        [Required]
        public string ProductName { get; set; }

        public int? AntibioticManageLevelId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsEssential { get; set; }

        public string DosageForm { get; set; }

        public decimal Ddd { get; set; }

        public decimal UnitCost { get; set; }

        public string UnitName { get; set; }

        public virtual AntibioticManageLevels AntibioticManageLevels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientPrescriptionDetails> OutPatientPrescriptionDetails { get; set; }
    }
}
