namespace PHMS2Domain.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PHMS2DomainContext : DbContext
    {
        public PHMS2DomainContext()
            : base("name=PHMS2DomainString")
        {
        }

        public virtual DbSet<AntibioticManageLevels> AntibioticManageLevels { get; set; }
        public virtual DbSet<CostCategories> CostCategories { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<DrugMaintenances> DrugMaintenances { get; set; }
        public virtual DbSet<OutPatientPrescriptionDetails> OutPatientPrescriptionDetails { get; set; }
        public virtual DbSet<OutPatientPrescriptions> OutPatientPrescriptions { get; set; }
        public virtual DbSet<OutPatientReceiptDetails> OutPatientReceiptDetails { get; set; }
        public virtual DbSet<OutPatientReceipts> OutPatientReceipts { get; set; }
        public virtual DbSet<RegisterCategories> RegisterCategories { get; set; }
        public virtual DbSet<Usages> Usages { get; set; }

        public virtual DbSet<Registers> Registers { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CostCategories>()
                .HasMany(e => e.OutPatientReceiptDetails)
                .WithRequired(e => e.CostCategories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departments>()
                .HasMany(e => e.OutPatientPrescriptions)
                .WithRequired(e => e.Departments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctors>()
                .HasMany(e => e.OutPatientPrescriptions)
                .WithRequired(e => e.Doctors)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DrugMaintenances>()
                .Property(e => e.Ddd)
                .HasPrecision(18, 4);

            modelBuilder.Entity<DrugMaintenances>()
                .Property(e => e.UnitCost)
                .HasPrecision(18, 4);

            modelBuilder.Entity<DrugMaintenances>()
                .HasMany(e => e.OutPatientPrescriptionDetails)
                .WithRequired(e => e.DrugMaintenances)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutPatientPrescriptionDetails>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OutPatientPrescriptions>()
                .HasMany(e => e.OutPatientPrescriptionDetails)
                .WithRequired(e => e.OutPatientPrescriptions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutPatientReceiptDetails>()
                .Property(e => e.Cost)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OutPatientReceipts>()
                .HasMany(e => e.OutPatientPrescriptions)
                .WithRequired(e => e.OutPatientReceipts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutPatientReceipts>()
                .HasMany(e => e.OutPatientReceiptDetails)
                .WithRequired(e => e.OutPatientReceipts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RegisterCategories>()
                .HasMany(e => e.Registers)
                .WithRequired(e => e.RegisterCategories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Registers>()
                .HasMany(e => e.OutPatientPrescriptions)
                .WithRequired(e => e.Registers)
                .WillCascadeOnDelete(false);
        }
    }
}
