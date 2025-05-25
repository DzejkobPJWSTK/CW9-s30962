using MyApp.DTOs;

namespace MyApp.Services;

public interface IDbService
{
    Task<PatientDetailsDto> GetPatientDetailsAsync(int idPatient);
    Task AddPrescriptionAsync(CreatePrescriptionDto dto);
}