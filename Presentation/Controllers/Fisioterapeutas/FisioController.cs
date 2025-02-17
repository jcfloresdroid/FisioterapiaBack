using Core.Features;
using Core.Features.Fisioterapeutas.command;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Catalogos;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class FisioController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public FisioController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Lista de fisioterapeutas paginado
    /// </summary>
    [HttpGet("{page}")]
    public async Task<GetFisioterapeutaResponse> GetFisio([FromQuery] bool onlyActive, int page)
    {
        return await _mediator.Send(new GetFisioterapeutas(){ OnlyActive = onlyActive, Pagina = page});
    }
    
    /// <summary>
    /// Lista de los fisioterapeutas
    /// </summary>
    [HttpGet("List")]
    public async Task<List<SelectFisioResponse>> getListFisio()
    {
        return await _mediator.Send(new SelectFisio() );
    }
    
    /// <summary>
    /// Mi equipo
    /// </summary>
    [HttpGet("team")]
    public async Task<List<MyTeamResponse>> getTeam()
    {
        return await _mediator.Send(new MyTeam() );
    }
    
    /// <summary>
    /// Buscador de fisioterapeutas
    /// </summary>
    [HttpGet("Buscador")]
    public async Task<SearchFisiosResponse> getSearch([FromQuery] int pagina, [FromQuery] string nombre, [FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new SearchFisios() { Pagina = pagina, Nombre = nombre, OnlyActive = onlyActive });
    }
    
    /// <summary>
    /// ultimos fisioterapeutas
    /// </summary>
    [HttpGet("Ultimos")]
    public async Task<List<UltimosFisiosResponse>> ultimos()
    {
        return await _mediator.Send(new UltimosFisios());
    }
    
    /// <summary>
    /// Registra a un nuevo fisioterapeuta
    /// </summary>
    [HttpPost()]
    public async Task<IActionResult> PostFisio([FromForm] PostFisioterapeutas command)
    {
        await _mediator.Send(command);
        return Ok("Se registro el fisioterapeuta correctamente");
    }
    
    /// <summary>
    /// Modificar el estatus de un fisioterapeuta
    /// </summary>
    [HttpPut("Estatus")]
    public async Task<IActionResult> StatusFisioterapeuta([FromBody] StatusFisios command)
    {
        await _mediator.Send(command);
        return Ok("Se cambio el estado correctamente");
    }
}