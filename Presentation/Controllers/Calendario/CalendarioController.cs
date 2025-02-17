using Core.Features.Calendario;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class CalendarioController
{
    private readonly IMediator _mediator;

    public CalendarioController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Devuelve las citas y los pacientes de una fecha determinada
    /// </summary>
    [HttpGet()]
    public async Task<ObtenerDatosFechaResponse> getCalendario([FromQuery] DateTime fecha)
    {
        return await _mediator.Send(new ObtenerDatosFecha(){ Fecha = fecha});
    }
}