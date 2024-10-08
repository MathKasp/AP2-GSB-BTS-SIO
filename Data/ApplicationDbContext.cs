using System;
using newEmpty.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NewEmpty.Data;

public class ApplicationDbContext : IdentityDbContext<Medecin>
{
    public DbSet<Patient> Patients => Set<Patient>();

    // public DbSet<Medecin> Medecins => Set<Medecin>();

    public DbSet<Allergie> Allergies => Set<Allergie>();

    public DbSet<Antecedent> Antecedents => Set<Antecedent>();

    public DbSet<Medicament> Medicaments => Set<Medicament>();

    public DbSet<Ordonnance> Ordonnances => Set<Ordonnance>();


    // Constructeur 
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    // définition des relations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Allergies)
            .WithMany(a => a.Patients)
            .UsingEntity(j => j.ToTable("PatientAllergie"));

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Antecedents)
            .WithMany(a => a.Patients)
            .UsingEntity(j => j.ToTable("PatientAntecedent"));

        modelBuilder.Entity<Allergie>()
            .HasMany(a => a.Medicaments)
            .WithMany(m => m.Allergies)
            .UsingEntity(j => j.ToTable("MedicamentAllergie"));

        modelBuilder.Entity<Antecedent>()
            .HasMany(a => a.Medicaments)
            .WithMany(m => m.Antecedents)
            .UsingEntity(j => j.ToTable("MedicamentAntecedent"));

        modelBuilder.Entity<Ordonnance>()
            .HasOne(a => a.Medecin)
            .WithMany(m => m.Ordonnances)
            .HasForeignKey(t => t.MedecinId);

        modelBuilder.Entity<Ordonnance>()
            .HasOne(o => o.Patient)
            .WithMany(p => p.Ordonnances);

        base.OnModelCreating(modelBuilder);
    
    }

}