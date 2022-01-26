using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AggregatorBankingServices.KnowledgeBase.Models
{
    public partial class KnowledgeBaseContext : DbContext
    {
        public KnowledgeBaseContext()
        {
        }

        public KnowledgeBaseContext(DbContextOptions<KnowledgeBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Domain> Domains { get; set; } = null!;
        public virtual DbSet<DomainValue> DomainValues { get; set; } = null!;
        public virtual DbSet<Fact> Facts { get; set; } = null!;
        public virtual DbSet<Rule> Rules { get; set; } = null!;
        public virtual DbSet<Variable> Variables { get; set; } = null!;
        public virtual DbSet<VariablesType> VariablesTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\santa\\source\\repos\\AggregatorBankingServices\\AggregatorBankingServices\\KnowledgeBase\\KnowledgeBase.mdf;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Cyrillic_General_100_CS_AS");

            modelBuilder.Entity<Domain>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Domains__737584F724807270");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DomainValue>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__DomainVa__737584F71D5610E8");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.DomainName)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.DomainNameNavigation)
                    .WithMany(p => p.DomainValues)
                    .HasForeignKey(d => d.DomainName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DomainVal__Domai__25869641");
            });

            modelBuilder.Entity<Fact>(entity =>
            {
                entity.Property(e => e.DomainValueName)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.VariableName)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.DomainValueNameNavigation)
                    .WithMany(p => p.Facts)
                    .HasForeignKey(d => d.DomainValueName)
                    .HasConstraintName("FK__Facts__DomainVal__2F10007B");

                entity.HasOne(d => d.VariableNameNavigation)
                    .WithMany(p => p.Facts)
                    .HasForeignKey(d => d.VariableName)
                    .HasConstraintName("FK__Facts__VariableN__2E1BDC42");
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdditionalRule)
                    .WithMany(p => p.InverseAdditionalRule)
                    .HasForeignKey(d => d.AdditionalRuleId)
                    .HasConstraintName("FK__Rules__Additiona__33D4B598");

                entity.HasOne(d => d.Fact)
                    .WithMany(p => p.RuleFacts)
                    .HasForeignKey(d => d.FactId)
                    .HasConstraintName("FK__Rules__FactId__31EC6D26");

                entity.HasOne(d => d.FactResult)
                    .WithMany(p => p.RuleFactResults)
                    .HasForeignKey(d => d.FactResultId)
                    .HasConstraintName("FK__Rules__FactResul__32E0915F");
            });

            modelBuilder.Entity<Variable>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Variable__737584F7B1DBA5C9");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.DomainName)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Question).IsUnicode(false);

                entity.Property(e => e.VariableTypeName)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.DomainNameNavigation)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.DomainName)
                    .HasConstraintName("FK__Variables__Domai__2A4B4B5E");

                entity.HasOne(d => d.VariableTypeNameNavigation)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.VariableTypeName)
                    .HasConstraintName("FK__Variables__Varia__2B3F6F97");
            });

            modelBuilder.Entity<VariablesType>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Variable__737584F73B6CD8D2");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
