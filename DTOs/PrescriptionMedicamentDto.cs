using System.ComponentModel.DataAnnotations;

namespace MyApp.DTOs;

public class PrescriptionMedicamentDto
{
    [Required]
    public int IdMedicament { get; set; }

    [Required]
    public int Dose { get; set; }

    [Required]
    public string Details { get; set; } = null!;
}