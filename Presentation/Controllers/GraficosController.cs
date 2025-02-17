using Core.Features.Citas.queries;
using Core.Features.Graficos.queries;
using iTextSharp.text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class GraficosController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public GraficosController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Datos de citas
    /// </summary>
    [HttpGet("Citas")]
    public async Task<GraphicsDateResponse> getDateGraphics()
    {
        return await _mediator.Send(new GraphicsDate());
    }
    
    /// <summary>
    /// Datos de los pacientes
    /// </summary>
    [HttpGet("Pacientes")]
    public async Task<GraphicsPatientResponse> getPatientGraphics()
    {
        return await _mediator.Send(new GraphicsPatient());
    }
    
    [HttpGet("TopFisio")]
    public async Task<List<FisioMesResponse>> getFisioMes()
    {
        return await _mediator.Send(new FisiosMes());
    }
    
    [HttpGet("MotivosAlta")]
    public async Task<List<GraphicsMotivoAltaResponse>> getMotivo([FromQuery] int meses)
    {
        return await _mediator.Send(new GraphicsMotivoAlta(){ Meses = meses });
    }
    
    [HttpGet("Edades")]
    public async Task<EdadesPacienteResponse> getEdades([FromQuery] int meses)
    {
        return await _mediator.Send(new EdadesPaciente(){ Meses = meses });
    }
    
    [HttpGet("Sexo")]
    public async Task<SexoPacienteResponse> getSexo([FromQuery] int meses)
    {
        return await _mediator.Send(new SexoPaciente(){ Meses = meses });
    }
}