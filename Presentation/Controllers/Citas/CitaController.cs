using Core.Features.Citas.command;
using Core.Features.Citas.queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class CitaController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CitaController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Citas del día
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet()]
    public async Task<List<GetDailyDateResponse>> getDateDaily()
    {
        return await _mediator.Send(new GetDailyDate());
    }
    
    /// <summary>
    /// Verifica si el paciente tiene un diagnostico activo
    /// </summary>
    [HttpGet("Diagnostico/{pancienteId}/{citaId}")]
    public async Task<GoDiagnosticoResponse> getDiagnostico(string pancienteId, string citaId)
    {
        return await _mediator.Send(new GoDiagnostico(){ PacienteId = pancienteId, CitaId = citaId });
    }
    
    /// <summary>
    /// Citas del paciente
    /// </summary>
    [HttpGet("Paciente")]
    public async Task<List<GetDateResponse>> getDate([FromQuery] string pancienteId)
    {
        return await _mediator.Send(new GetDate(){ PacienteId = pancienteId});
    }
    
    /// <summary>
    /// Crea una cita para el paciente
    /// </summary>
    [HttpPost()]
    public async Task<IActionResult> PostDate([FromBody] PostDate command)
    {
        await _mediator.Send(command);
        return Ok("Se creo la cita correctamente");
    }
    
    /// <summary>
    /// Modifica una cita
    /// </summary>
    [HttpPatch()]
    public async Task<IActionResult> PatchDate([FromBody] ModifyDate command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico la cita correctamente");
    }
}