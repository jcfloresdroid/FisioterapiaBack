using Core.Features.Pacientes.Command;
using Core.Features.Pacientes.queries;
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
public class ExpedienteController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ExpedienteController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Expediente del paciente
    /// </summary>
    [HttpGet()]
    public async Task<GetExpedientResponse> getExpedient([FromQuery] string paciente)
    {
        return await _mediator.Send(new GetExpedient() { PacienteId = paciente });
    }
    
    /// <summary>
    /// Se crea el expediente del usuario
    /// </summary>
    [HttpPost()]
    public async Task<IActionResult> PostExpedient([FromBody] PostExpedient command)
    {
        await _mediator.Send(command);
        return Ok("Se agregaron los datos correctamente");
    }
    
    /// <summary>
    /// Modificacion del expediente
    /// </summary>
    [HttpPatch()]
    public async Task<IActionResult> ModificacionExpediente([FromBody] ModificacionExpediente command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico el paciente correctamente");
    }
}