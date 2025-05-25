using System.ComponentModel.DataAnnotations;

namespace MyApp.DTOs;

public class PatientDto
{
    public int? IdPatient { get; set; }  // jeśli istnieje, szukamy pacjenta; jeśli nie, tworzymy nowego

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime Birthdate { get; set; }
}