namespace PHMS2Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutPatientReceiptDetails
    {
        public Guid OutPatientReceiptDetailsId { get; set; }

        public Guid OutPatientReceiptId { get; set; }

        public Guid FPMXID_Origin { get; set; }

        public int CostCategoryId { get; set; }

        public decimal Cost { get; set; }

        public virtual CostCategories CostCategories { get; set; }

        public virtual OutPatientReceipts OutPatientReceipts { get; set; }
    }
}
