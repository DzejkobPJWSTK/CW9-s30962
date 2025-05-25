using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.DTOs;
using MyApp.Exceptions;
using MyApp.Models;
using MyApp.Services;

public class DbService(AppDbContext dbContext) : IDbService
{
    public async Task AddPrescriptionAsync(CreatePrescriptionDto newPrescriptionDto)
    {
        if (newPrescriptionDto.Medicaments.Count > 10)
            throw new InvalidOperationException("Recepta może zawierać maksymalnie 10 leków.");

        if (newPrescriptionDto.DueDate < newPrescriptionDto.Date)
            throw new InvalidOperationException("Data ważności recepty musi być późniejsza.");

        var doctor = await dbContext.Doctors.FindAsync(newPrescriptionDto.IdDoctor)
                     ?? throw new NotFoundException("Nie znaleziono lekarza.");

        var medicamentList = await dbContext.Medicaments
            .Where(m => newPrescriptionDto.Medicaments.Select(x => x.IdMedicament).Contains(m.IdMedicament))
            .ToListAsync();

        if (medicamentList.Count != newPrescriptionDto.Medicaments.Count)
            throw new InvalidOperationException("Nie znaleziono leków.");

        Patient patient;

        if (newPrescriptionDto.Patient.IdPatient.HasValue)
        {
            patient = await dbContext.Patients.FindAsync(newPrescriptionDto.Patient.IdPatient.Value)
                      ?? new Patient
                      {
                          FirstName = newPrescriptionDto.Patient.FirstName,
                          LastName = newPrescriptionDto.Patient.LastName,
                          Birthdate = newPrescriptionDto.Patient.Birthdate
                      };
        }
        else
        {
            patient = new Patient
            {
                FirstName = newPrescriptionDto.Patient.FirstName,
                LastName = newPrescriptionDto.Patient.LastName,
                Birthdate = newPrescriptionDto.Patient.Birthdate
            };
            dbContext.Patients.Add(patient);
        }

        var newPrescription = new Prescription
        {
            Date = newPrescriptionDto.Date,
            DueDate = newPrescriptionDto.DueDate,
            Doctor = doctor,
            Patient = patient,
            PrescriptionMedicaments = newPrescriptionDto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        dbContext.Prescriptions.Add(newPrescription);
        await dbContext.SaveChangesAsync();
    }

    public async Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId)
    {
        var patient = await dbContext.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId)
            ?? throw new NotFoundException("Nie znaleziono pacjenta o podanym identyfikatorze.");

        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(pr => new PrescriptionDto
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName,
                        Email = pr.Doctor.Email
                    },
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDetailsDto
                    {
                        IdMedicament = pm.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
        };
    }
}
