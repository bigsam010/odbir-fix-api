using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OdbirReportingFix.Models
{
    public partial class odbir_dbContext : DbContext
    {
        public odbir_dbContext()
        {
        }

        public odbir_dbContext(DbContextOptions<odbir_dbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<LandUseTransactionLogs> LandUseTransactionLogs { get; set; }
        public virtual DbSet<Revenues> Revenues { get; set; }

        public virtual DbSet<TaxStationRevenueTargets> TaxStationRevenueTargets { get; set; }

        public virtual DbSet<TransactionLogs> TransactionLogs { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LandUseTransactionLogs>(entity =>
  {
      entity.Property(e => e.Id)
          .HasMaxLength(128)
          .ValueGeneratedNever();

      entity.Property(e => e.Mda).HasColumnName("MDA");

      entity.Property(e => e.Orin).HasColumnName("ORIN");

      entity.Property(e => e.TransactionDate).HasColumnType("datetime");
  });



            modelBuilder.Entity<Revenues>(entity =>
            {
                entity.HasIndex(e => e.TaxStationRevenueTargetId)
                    .HasName("IX_TaxStationRevenueTargetId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.TaxStationRevenueTarget)
                    .WithMany(p => p.Revenues)
                    .HasForeignKey(d => d.TaxStationRevenueTargetId)
                    .HasConstraintName("FK_dbo.Revenues_dbo.TaxStationRevenueTargets_TaxStationRevenueTargetId");
            });





            modelBuilder.Entity<TaxStationRevenueTargets>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");
            });



            modelBuilder.Entity<TransactionLogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Mda).HasColumnName("MDA");

                entity.Property(e => e.Orin).HasColumnName("ORIN");

                entity.Property(e => e.PaymentDateTime).HasColumnType("datetime");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            });


            //modelBuilder.Entity<SampleLog>(entity =>
            //{
            //    entity.HasKey(e => e.Id)
            //      .HasName("PK__samplelo__3213E83FCD5ABB00");
            //    entity.Property(e => e.Id);
            //    entity.ToTable


            //    entity.Property(e => e.Mda).HasColumnName("MDA");

            //    entity.Property(e => e.Orin).HasColumnName("ORIN");

            //    entity.Property(e => e.PaymentDateTime).HasColumnType("datetime");

            //    entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            //});

        }
    }
}
