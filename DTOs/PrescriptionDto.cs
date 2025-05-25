namespace MyApp.DTOs;

public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public List<MedicamentDetailsDto> Medicaments { get; set; } = new();

    public DoctorDto Doctor { get; set; } = null!;
}