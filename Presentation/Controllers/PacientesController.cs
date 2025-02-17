using Core.Features.Fisioterapeutas.command;
using Core.Features.Pacientes.command;
using Core.Features.Pacientes.Command;
using Core.Features.Pacientes.queries;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly FisioContext _context;

    public PacientesController(IMediator mediator, FisioContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Lista de pacientes paginado
    /// </summary>
    [HttpGet()]
    public async Task<GetPatientsResponse> getPatients([FromQuery] int pagina, [FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new GetPatients() { Pagina = pagina, OnlyActive = onlyActive });
    }

    /// <summary>
    /// Obtiene la Data de un paciente en especifico
    /// </summary>
    [HttpGet("Data")]
    public async Task<PatientDataResponse> getPatient([FromQuery] string pacienteId)
    {
        return await _mediator.Send(new PatientData() { PacienteId = pacienteId });
    }

    /// <summary>
    /// Buscador pacientes
    /// </summary>
    [HttpGet("Buscador")]
    public async Task<SearchPatientResponse> getSearch([FromQuery] int pagina, [FromQuery] string nombre,
        [FromQuery] bool onlyActive)
    {
        return await _mediator.Send(new SearchPatients() { Pagina = pagina, Nombre = nombre, OnlyActive = onlyActive });
    }

    /// <summary>
    /// Ultimos pacientes registrados
    /// </summary>
    [HttpGet("Ultimos")]
    public async Task<List<LastPatientsResponse>> getLastPatients()
    {
        return await _mediator.Send(new LastPatients());
    }

    /// <summary>
    /// Registrar un nuevo paciente
    /// </summary>
    [HttpPost()]
    public async Task<CreatePatientResponse> PostPatient([FromForm] CreatePatient command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Estado del paciente
    /// </summary>
    [HttpPut("Estatus")]
    public async Task<IActionResult> StatusPaciente([FromBody] StatusPaciente command)
    {
        await _mediator.Send(command);
        return Ok("Se cambio el estado correctamente");
    }

    /// <summary>
    /// Modificar data del paciente
    /// </summary>
    [HttpPatch()]
    public async Task<IActionResult> ModificacionPaciente([FromForm] ModificarPaciente command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico el paciente correctamente");
    }
}