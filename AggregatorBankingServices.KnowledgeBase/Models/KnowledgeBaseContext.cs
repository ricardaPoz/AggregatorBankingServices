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
                    .HasName("PK__Domains__737584F7639E75DB");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DomainValue>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__DomainVa__737584F74D87DB9F");

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
                    .HasConstraintName("FK__DomainVal__Domai__38996AB5");
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Facts__DomainVal__4222D4EF");

                entity.HasOne(d => d.VariableNameNavigation)
                    .WithMany(p => p.Facts)
                    .HasForeignKey(d => d.VariableName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Facts__VariableN__412EB0B6");
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Fact)
                    .WithMany(p => p.RuleFacts)
                    .HasForeignKey(d => d.FactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rules__FactId__44FF419A");

                entity.HasOne(d => d.FactResult)
                    .WithMany(p => p.RuleFactResults)
                    .HasForeignKey(d => d.FactResultId)
                    .HasConstraintName("FK__Rules__FactResul__45F365D3");
            });

            modelBuilder.Entity<Variable>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Variable__737584F7A7F0EE27");

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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Variables__Domai__3D5E1FD2");

                entity.HasOne(d => d.VariableTypeNameNavigation)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.VariableTypeName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Variables__Varia__3E52440B");
            });

            modelBuilder.Entity<VariablesType>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Variable__737584F70A2A011F");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
