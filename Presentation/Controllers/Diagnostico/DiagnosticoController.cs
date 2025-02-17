using Core.Features.Diagnostico.command;
using Core.Features.Diagnostico.queries;
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
public class DiagnosticoController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public DiagnosticoController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Diagnostico del paciente
    /// </summary>
    [HttpGet()]
    public async Task<GeneralDiagnosticResponse> DiagnosticGet([FromQuery] string diagnostico)
    {
        return await _mediator.Send(new DiagnosticGet() { DiagnosticoId = diagnostico });
    }
    
    /// <summary>
    /// Ultima exploracion del paciente
    /// </summary>
    [HttpGet("UltimaExploracion")]
    public async Task<GetUltimaExploracionResponse> GetUltimo([FromQuery] string paciente)
    {
        return await _mediator.Send(new GetUltimaExploracion() { PacienteId = paciente });
    }
    
    /// <summary>
    /// ¿Diagnostico activo del paciente?
    /// </summary>
    [HttpGet("Status")]
    public async Task<DiagnosticActiveResponse> ActiveDiagnostic([FromQuery] string expediente)
    {
        return await _mediator.Send(new DiagnosticActive() { ExpedienteId = expediente });
    }
    
    /// <summary>
    /// Revision (Citas)
    /// </summary>
    [HttpGet("Revision")]
    public async Task<List<GetRevisionesResponse>> GetReview([FromQuery] string diagnostico)
    {
        return await _mediator.Send(new GetRevisiones() { DiagnosticoId = diagnostico });
    }
    
    /// <summary>
    /// Crear un nuevo diagnostico
    /// </summary>
    [HttpPost()]
    public async Task<IActionResult> PostDiagnostic([FromBody] GeneralDiagnosticPost command)
    {
        await _mediator.Send(command);
        return Ok("Se creo el diagnostico correctamente");
    }
    
    /// <summary>
    /// Se crea la revision del usuario (Citas)
    /// </summary>
    [HttpPost("Revision")]
    public async Task<IActionResult> PostReview([FromBody] PostRevisiones command)
    {
        await _mediator.Send(command);
        return Ok("Se creo la revision correctamente");
    }
    
    /// <summary>
    /// Finalizar diagnostico
    /// </summary>
    [HttpPut("Finalizar")]
    public async Task<IActionResult> PatchDiagnotico([FromBody] FinalizarDiagnostico command)
    {
        await _mediator.Send(command);
        return Ok("Se finalizo correctamente el diagnostico");
    }
}