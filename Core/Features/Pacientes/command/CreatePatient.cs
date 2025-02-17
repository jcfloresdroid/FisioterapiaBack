using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Pacientes.Command;

public record CreatePatient : IRequest<CreatePatientResponse>
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Institucion { get; set; }
    public string Domicilio { get; set; }
    public string Ocupacion { get; set; }
    public string Telefono { get; set; }
    public string? Notas { get; set; }
    public string EstadoCivilId { get; set; }
    public string FisioterapeutaId { get; set; }
    public bool Sexo { get; set; }
    public bool TipoPaciente { get; set; }
    public int CodigoPostal { get; set; }
    public DateTime Edad { get; set; } 
    public IFormFile? Foto { get; set; }
}

public class CreatePatientHandler : IRequestHandler<CreatePatient, CreatePatientResponse>
{
    private readonly FisioContext _context;
    private readonly IPacienteValidator _validator;
    private readonly IConvertType _convertType;
    private readonly IExistResource _existResource;
    
    public CreatePatientHandler(FisioContext context, IPacienteValidator validator, IConvertType convertType, IExistResource existResource)
    {
        _context = context;
        _validator = validator;
        _convertType = convertType;
        _existResource = existResource;
    }
    
    public async Task<CreatePatientResponse> Handle(CreatePatient request, CancellationToken cancellationToken)
    {
        // Validacion
        await _validator.addPatient(request);
        
        // Verificar si existe el estado civil y el fisioterapeuta
        await _existResource.ExistEstadoCivil(request.EstadoCivilId);
        await _existResource.ExistFisioterapeuta(request.FisioterapeutaId);
        
        if (request.Edad.Year >= FormatDate.DateLocal().Year)
            throw new BadRequestException(Message.PACI_0015);
        
        var patient = new Paciente() {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Institucion = request.Institucion,
            Domicilio = request.Domicilio,
            Ocupacion = request.Ocupacion,
            Telefono = request.Telefono,
            Notas = request.Notas,
            Sexo = request.Sexo,
            TipoPaciente = request.TipoPaciente,
            Status = true,
            Edad = request.Edad,
            FechaRegistro = FormatDate.DateLocal().Date,
            CodigoPostal = request.CodigoPostal,
            EstadoCivilId = request.EstadoCivilId.HashIdInt(),
            FisioterapeutaId = request.FisioterapeutaId.HashIdInt(),
            Foto = request.Foto == null ? await _convertType.profilePicture() : await _convertType.uploadFile(request.Foto)
        };

        await _context.Pacientes.AddAsync(patient);
        await _context.SaveChangesAsync();

        return await Task.FromResult(new CreatePatientResponse()
        {
            PacienteId = patient.PacienteId.HashId()
        });
    }
}

public record CreatePatientResponse
{
    public string PacienteId { get; set; }
}