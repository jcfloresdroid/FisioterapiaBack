using Core.Features.Catalogos.command;
using Core.Features.Catalogos.queries;
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
public class CatalogoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogoController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Catalogo de especialidades
    /// </summary>
    [AllowAnonymous]
    [HttpGet("Especialidades")]
    public async Task<List<GetEspecialidadesResponse>> getEspecialidades([FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetEspecialidades(){ OnlyActive = onlyActive});
    }
    
    /// <summary>
    /// Catalogo de estado Civil
    /// </summary>
    [HttpGet("EstadoCivil")]
    public async Task<List<GetEstadoCivilResponse>> getEstadoCivil([FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetEstadoCivil(){ OnlyActive = onlyActive});
    }
    
    /// <summary>
    /// Catalogo de flujo Vaginal
    /// </summary>
    [HttpGet("FlujoVaginal")]
    public async Task<List<GetFlujoVaginalResponse>> getFlujoVaginal([FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetFlujoVaginal(){ OnlyActive = onlyActive});
    }
    
    /// <summary>
    /// Catalogo de motivo de alta
    /// </summary>
    [HttpGet("MotivoAlta")]
    public async Task<List<GetMotivoAltaResponse>> getMotivoAlta([FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetMotivoAlta(){ OnlyActive = onlyActive});
    }
    
    /// <summary>
    /// Catálogo de patologías
    /// </summary>
    [HttpGet("Patologias")]
    public async Task<List<GetPatologiasResponse>> getPatologias([FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetPatologias(){ OnlyActive = onlyActive});
    }
    
    /// <summary>
    /// Catalogo de servicios
    /// </summary>
    [HttpGet("Servicios")]
    public async Task<List<GetServiciosResponse>> getServicios([FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetServicios(){ OnlyActive = onlyActive});
    }
    
    /// <summary>
    /// Catalogo de tipos de anticonceptivos
    /// </summary>
    [HttpGet("TipoAnticonceptivo")]
    public async Task<List<GetAnticonceptivoResponse>> getAnticonceptivo([FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetAnticonceptivo(){ OnlyActive = onlyActive});
    }
    
    /// <summary>
    /// Registrar una nueva especialidad
    /// </summary>
    [HttpPost("Especialidades")]
    public async Task<IActionResult> PostEspecialidades([FromBody] PostEspecialidades command)
    {
        await _mediator.Send(command);
        return Ok("Se creo correctamente");
    }
    
    /// <summary>
    /// Registra un nuevo estado civil
    /// </summary>
    [HttpPost("EstadoCivil")]
    public async Task<IActionResult> PostEstadoCivil([FromBody] PostEstadoCivil command)
    {
        await _mediator.Send(command);
        return Ok("Se creo correctamente");
    }
    
    /// <summary>
    /// Registra un nuevo dato en flujo vaginal
    /// </summary>
    [HttpPost("FlujoVaginal")]
    public async Task<IActionResult> PostFlujoVaginal([FromBody] PostFlujoVaginal command)
    {
        await _mediator.Send(command);
        return Ok("Se creo correctamente");
    }
    
    /// <summary>
    /// Registra un nuevo dato en motivo alta
    /// </summary>
    [HttpPost("MotivoAlta")]
    public async Task<IActionResult> PostMotivoAlta([FromBody] PostMotivoAlta command)
    {
        await _mediator.Send(command);
        return Ok("Se creo correctamente");
    }
    
    /// <summary>
    /// Registra un nuevo dato en patologías
    /// </summary>
    [HttpPost("Patologias")]
    public async Task<IActionResult> PostPatologias([FromBody] PostPatologias command)
    {
        await _mediator.Send(command);
        return Ok("Se creo correctamente");
    }
    
    /// <summary>
    /// Registra un nuevo servicio
    /// </summary>
    [HttpPost("Servicios")]
    public async Task<IActionResult> PostServicio([FromBody] PostServicios command)
    {
        await _mediator.Send(command);
        return Ok("Se creo correctamente");
    }
    
    /// <summary>
    /// Registra un nuevo tipo de anticonceptivo
    /// </summary>
    [HttpPost("TipoAnticonceptivo")]
    public async Task<IActionResult> PostTivoAnticonceptivo([FromBody] PostAnticonceptivos command)
    {
        await _mediator.Send(command);
        return Ok("Se creo correctamente");
    }
    
    /// <summary>
    /// Modifica una especialidad
    /// </summary>
    [HttpPatch("Especialidades")]
    public async Task<IActionResult> PatchEspecialidades([FromBody] PutEspecialidades command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico correctamente");
    }
    
    /// <summary>
    /// Modifica un estado civil
    /// </summary>
    [HttpPatch("EstadoCivil")]
    public async Task<IActionResult> PatchEstadoCivil([FromBody] PutEstadoCivil command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico correctamente");
    }
    
    /// <summary>
    /// Modifica un campo de flujo vaginal
    /// </summary>
    [HttpPatch("FlujoVaginal")]
    public async Task<IActionResult> PatchFlujo([FromBody] PutFlujoVaginal command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico correctamente");
    }
    
    /// <summary>
    /// Modifica un campo de motivo de alta
    /// </summary>
    [HttpPatch("MotivoAlta")]
    public async Task<IActionResult> PatchMotivoAlta([FromBody] PutMotivoAlta command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico correctamente");
    }
    
    /// <summary>
    /// Modifica un campo de patologías
    /// </summary>
    [HttpPatch("Patologias")]
    public async Task<IActionResult> PatchPatologias([FromBody] PutPatologias command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico correctamente");
    }
    
    /// <summary>
    /// Modifica un servicio
    /// </summary>
    [HttpPatch("Servicios")]
    public async Task<IActionResult> PatchServicios([FromBody] PutServicios command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico correctamente");
    }
    
    /// <summary>
    /// Modifica un campo de tipo de anticonceptivo
    /// </summary>
    [HttpPatch("TipoAnticonceptivo")]
    public async Task<IActionResult> PatchTipoAnticonceptivo([FromBody] PutAnticonceptivo command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico correctamente");
    }
}