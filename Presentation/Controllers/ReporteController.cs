using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Features.Reportes;
using MediatR;

namespace Presentation.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class ReporteController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ReporteController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Pdf Diagnostico
    /// </summary>
    [HttpGet()]
    public async Task<ReporteExpedienteResponse> getReport([FromQuery] string diagnosticoId)
    {
        return await _mediator.Send(new ReporteExpediente() { DiagnosticoId = diagnosticoId });
    }
    
    /*[HttpGet()]
    public async Task<IActionResult> getReport([FromQuery] string diagnosticoId)
    {
        var response = await _mediator.Send(new ReporteExpediente() { DiagnosticoId = diagnosticoId });
        return Ok(response.Reporte);
    }*/
}