using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

public partial class ErpDbContext : DbContext
{
    public ErpDbContext()
    {
    }

    public ErpDbContext(DbContextOptions<ErpDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Absence> Absences { get; set; }

    public virtual DbSet<Assiduite> Assiduites { get; set; }

    public virtual DbSet<Conge> Conges { get; set; }

    public virtual DbSet<Département> Départements { get; set; }

    public virtual DbSet<Horaire> Horaires { get; set; }

    public virtual DbSet<Paye> Payes { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Personnel> Personnel { get; set; }

    public virtual DbSet<Retard> Retards { get; set; }

    public virtual DbSet<Supplementaire> Supplementaires { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=ERPP;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.HasKey(e => e.Absenceid).HasName("absence_pkey");

            entity.ToTable("absence");

            entity.Property(e => e.Absenceid).HasColumnName("absenceid");
            entity.Property(e => e.Assiduiteid).HasColumnName("assiduiteid");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Justifiee).HasColumnName("justifiee");
            entity.Property(e => e.Totalheures).HasColumnName("totalheures");

            entity.HasOne(d => d.Assiduite).WithMany(p => p.Absences)
                .HasForeignKey(d => d.Assiduiteid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("absence_assiduiteid_fkey");
        });

        modelBuilder.Entity<Assiduite>(entity =>
        {
            entity.HasKey(e => e.Assiduiteid).HasName("assiduite_pkey");

            entity.ToTable("assiduite");

            entity.Property(e => e.Assiduiteid).HasColumnName("assiduiteid");
            entity.Property(e => e.Matricule).HasColumnName("matricule");
            entity.Property(e => e.Totalheuresabsence).HasColumnName("totalheuresabsence");
            entity.Property(e => e.Totalheurespermission).HasColumnName("totalheurespermission");
            entity.Property(e => e.Totalheurespresence).HasColumnName("totalheurespresence");
            entity.Property(e => e.Totalheuresretard).HasColumnName("totalheuresretard");
            entity.Property(e => e.Totalheuressupplementaires).HasColumnName("totalheuressupplementaires");

            entity.HasOne(d => d.MatriculeNavigation).WithMany(p => p.Assiduites)
                .HasForeignKey(d => d.Matricule)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("assiduite_matricule_fkey");
        });

        modelBuilder.Entity<Conge>(entity =>
        {
            entity.HasKey(e => e.Congeid).HasName("conge_pkey");

            entity.ToTable("conge");

            entity.Property(e => e.Congeid).HasColumnName("congeid");
            entity.Property(e => e.Datedebut).HasColumnName("datedebut");
            entity.Property(e => e.Datefin).HasColumnName("datefin");
            entity.Property(e => e.Matricule).HasColumnName("matricule");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.MatriculeNavigation).WithMany(p => p.Conges)
                .HasForeignKey(d => d.Matricule)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("conge_matricule_fkey");
        });

        modelBuilder.Entity<Département>(entity =>
        {
            entity.HasKey(e => e.Departementid).HasName("département_pkey");

            entity.ToTable("département");

            entity.Property(e => e.Departementid).HasColumnName("departementid");
            entity.Property(e => e.Nom)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Horaire>(entity =>
        {
            entity.HasKey(e => e.Horaireid).HasName("horaire_pkey");

            entity.ToTable("horaire");

            entity.Property(e => e.Horaireid).HasColumnName("horaireid");
            entity.Property(e => e.Assiduiteid).HasColumnName("assiduiteid");
            entity.Property(e => e.Heuredebut).HasColumnName("heuredebut");
            entity.Property(e => e.Heurefin).HasColumnName("heurefin");
            entity.Property(e => e.Totalheures).HasColumnName("totalheures");

            entity.HasOne(d => d.Assiduite).WithMany(p => p.Horaires)
                .HasForeignKey(d => d.Assiduiteid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("horaire_assiduiteid_fkey");
        });

        modelBuilder.Entity<Paye>(entity =>
        {
            entity.HasKey(e => e.Payeid).HasName("paye_pkey");

            entity.ToTable("paye");

            entity.Property(e => e.Payeid).HasColumnName("payeid");
            entity.Property(e => e.Datepaiement).HasColumnName("datepaiement");
            entity.Property(e => e.Matricule).HasColumnName("matricule");
            entity.Property(e => e.Nombredejours).HasColumnName("nombredejours");
            entity.Property(e => e.Periode).HasColumnName("periode");
            entity.Property(e => e.Prime).HasColumnName("prime");
            entity.Property(e => e.Salairebrut).HasColumnName("salairebrut");
            entity.Property(e => e.Salairenet).HasColumnName("salairenet");

            entity.HasOne(d => d.MatriculeNavigation).WithMany(p => p.Payes)
                .HasForeignKey(d => d.Matricule)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("paye_matricule_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Permissionid).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Assiduiteid).HasColumnName("assiduiteid");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Duree).HasColumnName("duree");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Assiduite).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.Assiduiteid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("permission_assiduiteid_fkey");
        });

        modelBuilder.Entity<Personnel>(entity =>
        {
            entity.HasKey(e => e.Matricule).HasName("personnel_pkey");

            entity.ToTable("personnel");

            entity.HasIndex(e => e.Email, "personnel_email_key").IsUnique();

            entity.Property(e => e.Matricule).HasColumnName("matricule");
            entity.Property(e => e.Adresse)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("adresse");
            entity.Property(e => e.Dateembauche).HasColumnName("dateembauche");
            entity.Property(e => e.Datenaissance).HasColumnName("datenaissance");
            entity.Property(e => e.Departementid).HasColumnName("departementid");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Mdp)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("mdp");
            entity.Property(e => e.Nom)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nom");
            entity.Property(e => e.Numerotelephone).HasColumnName("numerotelephone");
            entity.Property(e => e.Poste)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("poste");
            entity.Property(e => e.Prenom)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("prenom");
            entity.Property(e => e.Statutfamiliale)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("statutfamiliale");
            entity.Property(e => e.Typecontrat)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("typecontrat");

            entity.HasOne(d => d.Departement).WithMany(p => p.Personnel)
                .HasForeignKey(d => d.Departementid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("personnel_departementid_fkey");
        });

        modelBuilder.Entity<Retard>(entity =>
        {
            entity.HasKey(e => e.Retardid).HasName("retard_pkey");

            entity.ToTable("retard");

            entity.Property(e => e.Retardid).HasColumnName("retardid");
            entity.Property(e => e.Assiduiteid).HasColumnName("assiduiteid");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Duree).HasColumnName("duree");
            entity.Property(e => e.Totalheures).HasColumnName("totalheures");

            entity.HasOne(d => d.Assiduite).WithMany(p => p.Retards)
                .HasForeignKey(d => d.Assiduiteid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("retard_assiduiteid_fkey");
        });

        modelBuilder.Entity<Supplementaire>(entity =>
        {
            entity.HasKey(e => e.Supplementaireid).HasName("supplementaire_pkey");

            entity.ToTable("supplementaire");

            entity.Property(e => e.Supplementaireid).HasColumnName("supplementaireid");
            entity.Property(e => e.Assiduiteid).HasColumnName("assiduiteid");
            entity.Property(e => e.Heuredebut).HasColumnName("heuredebut");
            entity.Property(e => e.Heurefin).HasColumnName("heurefin");
            entity.Property(e => e.Totalheures).HasColumnName("totalheures");

            entity.HasOne(d => d.Assiduite).WithMany(p => p.Supplementaires)
                .HasForeignKey(d => d.Assiduiteid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("supplementaire_assiduiteid_fkey");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Tokenid).HasName("token_pkey");

            entity.ToTable("token");

            entity.Property(e => e.Tokenid).HasColumnName("tokenid");
            entity.Property(e => e.Expiration)
                .HasDefaultValueSql("(now() AT TIME ZONE 'GMT+1'::text)")
                .HasColumnName("expiration");
            entity.Property(e => e.Matricule).HasColumnName("matricule");
            entity.Property(e => e.Tokenvalue)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("tokenvalue");

            entity.HasOne(d => d.MatriculeNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.Matricule)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("token_matricule_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
