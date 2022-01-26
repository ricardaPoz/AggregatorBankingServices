using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AggregatorBankingServices.DataBase.Models
{
    public partial class BankProductDataBaseContext : DbContext
    {
        public BankProductDataBaseContext()
        {
        }

        public BankProductDataBaseContext(DbContextOptions<BankProductDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BankName> BankNames { get; set; } = null!;
        public virtual DbSet<Contribution> Contributions { get; set; } = null!;
        public virtual DbSet<Loan> Loans { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=BankProductDataBase;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Cyrillic_General_100_CS_AS");

            modelBuilder.Entity<BankName>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__BankName__737584F750A003EE");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contribution>(entity =>
            {
                entity.Property(e => e.Capitalization)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.DepositAmountFrom).HasColumnType("decimal(20, 3)");

                entity.Property(e => e.DepositAmountTo).HasColumnType("decimal(20, 3)");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.NameBank)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.PartialRemoval)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentInterest)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnType("decimal(11, 3)");

                entity.Property(e => e.Replenishment)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.NameBankNavigation)
                    .WithMany(p => p.Contributions)
                    .HasForeignKey(d => d.NameBank)
                    .HasConstraintName("FK__Contribut__NameB__71D1E811");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("Loan");

                entity.Property(e => e.ApplicationReview)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.IncomeVerification)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.LoanAmountFrom).HasColumnType("decimal(20, 3)");

                entity.Property(e => e.LoanAmountTo).HasColumnType("decimal(20, 3)");

                entity.Property(e => e.MandatoryInsurance)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.NameBank)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentFrequency)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnType("decimal(11, 3)");

                entity.Property(e => e.RequiredDocuments)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.TypePayment)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.NameBankNavigation)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.NameBank)
                    .HasConstraintName("FK__Loan__NameBank__7A672E12");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PK__User__5E55825A2D613BEA");

                entity.ToTable("User");

                entity.Property(e => e.Password)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Scoring)
                    .HasMaxLength(450)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
