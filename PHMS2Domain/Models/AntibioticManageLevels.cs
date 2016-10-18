namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AntibioticManageLevels
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AntibioticManageLevels()
        {
            DrugMaintenances = new HashSet<DrugMaintenances>();
        }

        [Key]
        public int AntibioticManageLevelId { get; set; }

        [Required]
        [StringLength(50)]
        public string AntibioticManageLevelName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DrugMaintenances> DrugMaintenances { get; set; }
    }
}
