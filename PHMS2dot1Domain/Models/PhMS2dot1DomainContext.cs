using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PhMS2dot1Domain.Models
{
    public class PhMS2dot1DomainContext : DbContext
    {
        private static readonly object locker = new object();
        public PhMS2dot1DomainContext(string connection) : base(connection)
        {
            //this.Configuration.LazyLoadingEnabled = false;
           

        }
        public PhMS2dot1DomainContext() : base("PHMS2dot1DomainString")
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Database.Initialize(false);

        }
        public DbSet<AntibioticLevel> AntibioticLevels { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DrugFee> DrugFees { get; set; }
        public DbSet<InPatientDrugRecord> InPatientDrugRecords { get; set; }
        public DbSet<DrugUnit> DrugUnits { get; set; }
        public DbSet<DrugUsage> DrugUsages { get; set; }
        public DbSet<InPatient> InPatients { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<OutPatient> OutPatients { get; set; }
        public DbSet<OutPatientCategory> OutPatientCategories { get; set; }
        public DbSet<OutPatientDrugRecord> OutPatientDrugRecords { get; set; }
        public DbSet<OutPatientPrescription> OutPatientPrescriptions { get; set; }

        public DbSet<ImportDataLog> ImportDataLogs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //定义Decimal精度
            modelBuilder.Entity<InPatientDrugRecord>()
                .Property(d => d.DDD)
                .HasPrecision(18, 4);
            modelBuilder.Entity<InPatientDrugRecord>()
               .Property(d => d.EffectiveConstituentAmount)
               .HasPrecision(18, 4);
            modelBuilder.Entity<DrugFee>()
                .Property(d => d.UnitPrice)
                .HasPrecision(18, 4);
            modelBuilder.Entity<DrugFee>()
                .Property(d => d.Quantity)
                .HasPrecision(18, 4);
            modelBuilder.Entity<DrugFee>()
                .Property(d => d.ActualPrice)
                .HasPrecision(18, 4);
            
            //OutPatientDrugRecord
            modelBuilder.Entity<OutPatientDrugRecord>()
                .Property(o => o.UnitPrice).HasPrecision(18, 4);
            modelBuilder.Entity<OutPatientDrugRecord>()
                .Property(o => o.Quantity).HasPrecision(18, 4);
            modelBuilder.Entity<OutPatientDrugRecord>()
                .Property(o => o.ActualPrice).HasPrecision(18, 4);
            modelBuilder.Entity<OutPatientDrugRecord>()
                .Property(o => o.EffectiveConstituentAmount).HasPrecision(18, 4);
            //InPatient 1: n InPatientDrugRecord ，级联删除
            modelBuilder.Entity<InPatient>()
                .HasMany(e => e.InPatientDrugRecords)
                .WithRequired(e => e.InPatient)
                .WillCascadeOnDelete(true);

            //OutPatient 1: n OutPatientDrugRecord, 级联删除
            modelBuilder.Entity<OutPatient>()
                .HasMany(o => o.OutPatientPrescriptions)
                .WithRequired(o => o.OutPatient)
                .WillCascadeOnDelete(true);

           

        }
    }
}
