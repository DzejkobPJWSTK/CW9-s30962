namespace MyApp.DTOs;

public class MedicamentDetailsDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Dose { get; set; }
}