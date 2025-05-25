using Microsoft.AspNetCore.Mvc;
using MyApp.DTOs;
using MyApp.Exceptions;
using MyApp.Services;

[ApiController]
[Route("Patient")]
public class PatientController : ControllerBase
{
    private readonly IDbService _dbService;

    public PatientController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("patient/{id}")]
    public async Task<IActionResult> GetPatientDetails([FromRoute] int id)
    {
        try
        {
            var patientDetails = await _dbService.GetPatientDetailsAsync(id);
            return Ok(patientDetails);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("prescription")]
    public async Task<IActionResult> AddPrescription([FromBody] CreatePrescriptionDto newPrescription)
    {
        try
        {
            await _dbService.AddPrescriptionAsync(newPrescription);
            return Ok("Dodano recepte!!");
        }
        catch (NotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}