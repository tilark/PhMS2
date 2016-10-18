namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RegisterCategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegisterCategories()
        {
            Registers = new HashSet<Registers>();
        }

        [Key]
        public int RegisterCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string RegisterCategoryName { get; set; }

        public string Remarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Registers> Registers { get; set; }
    }
}
