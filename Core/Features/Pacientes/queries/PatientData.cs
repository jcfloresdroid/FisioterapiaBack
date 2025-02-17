using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Pacientes.queries;

public record PatientData : IRequest<PatientDataResponse>
{
    public string PacienteId { get; set; }
}

public class DataPatientHandler : IRequestHandler<PatientData, PatientDataResponse>
{
    private readonly FisioContext _context;
    private readonly IExistResource _existResource;

    public DataPatientHandler(FisioContext context, IExistResource existResource)
    {
        _context = context;
        _existResource = existResource;
    }
    
    public async Task<PatientDataResponse> Handle(PatientData request, CancellationToken cancellationToken)
    {
        // Verificar que el paciente exista
        await _existResource.ExistPaciente(request.PacienteId);
        
        //Intentamos buscar al paciente
        var patient = await _context.Pacientes
            .AsNoTracking()
            .Include(x => x.Fisioterapeuta)
            .Include(u => u.Expediente)
            .FirstOrDefaultAsync(x => x.PacienteId == request.PacienteId.HashIdInt());
        
        var response = new PatientDataResponse()
        {
            PacienteId = patient.PacienteId.HashId(),
            Nombre = patient.Nombre,
            Apellido = patient.Apellido,
            Institucion = patient.Institucion,
            Domicilio = patient.Domicilio,
            Ocupacion = patient.Ocupacion,
            Telefono = patient.Telefono,
            FotoPerfil = patient.Foto,
            Notas = patient.Notas ?? "No hay notas",
            Sexo = patient.Sexo ? "Hombre" : "Mujer",
            TipoPaciente = patient.TipoPaciente ? "Interno" : "Externo",
            TipoPago = patient.TipoPaciente,
            Fisioterapeuta = patient.Fisioterapeuta.Nombre,
            Estatus = patient.Status,
            Verificado = patient.Expediente != null,
            FechaNacimiento = patient.Edad,
            Edad = FormatDate.DateToYear(patient.Edad.Date),
            CodigoPostal = patient.CodigoPostal,
            FisioterapeutaId = patient.FisioterapeutaId.Value.HashId(),
            EstadoCivilId = patient.EstadoCivilId.Value.HashId()
        };

        return response;
    }
}

public record PatientDataResponse
{
    public string PacienteId { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Institucion { get; set; }
    public string Domicilio { get; set; }
    public string Ocupacion { get; set; }
    public string Telefono { get; set; }
    public byte[] FotoPerfil { get; set; }
    public string Notas { get; set; }
    public string Sexo { get; set; }
    public string TipoPaciente { get; set; }
    public string Fisioterapeuta { get; set; }
    public bool Estatus { get; set; }
    public bool TipoPago { get; set; }
    public bool Verificado { get; set; } // Si tiene expediente completado
    public DateTime FechaNacimiento { get; set; }
    public int Edad { get; set; }
    public int CodigoPostal { get; set; }
    public string EstadoCivilId { get; set; }
    public string FisioterapeutaId { get; set; }
    
}