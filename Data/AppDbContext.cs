using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().HasData(new Patient
        {
            IdPatient = 1, FirstName = "Marek", LastName = "Janowski", Birthdate = new DateTime(2000,5,10)
        });

        modelBuilder.Entity<Doctor>().HasData(new Doctor
        {
            IdDoctor = 1, FirstName = "Aleksandra", LastName = "Kowalska", Email = "aleks@kow.pl"
        });

        modelBuilder.Entity<Medicament>().HasData(new Medicament
        {
            IdMedicament = 1, Name = "Ibuprofen", Description = "Przeciwbólowy", Type = "Tabletka"
        });
    }
}