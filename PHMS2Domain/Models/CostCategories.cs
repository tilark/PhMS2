namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CostCategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CostCategories()
        {
            OutPatientReceiptDetails = new HashSet<OutPatientReceiptDetails>();
        }

        [Key]
        public int CostCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CostCategoryName { get; set; }

        public bool IsDrug { get; set; }

        public string Remarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPatientReceiptDetails> OutPatientReceiptDetails { get; set; }
    }
}
