using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Pacientes.command;

public record ModificarPaciente : IRequest
{
    public string PacienteId { get; set; }
    
    public string? Nombre { get; set; }
    
    public string? Apellido { get; set; }
    
    public string? Institucion { get; set; }
    
    public string? Direccion { get; set; }
    
    public string? Ocupacion { get; set; }
    
    public string? Telefono { get; set; }
    
    public string? Notas { get; set; }
    
    public string? EstadoCivilId { get; set; }
    
    public string? FisioterapeutaId { get; set; }
    
    public int? CodigoPostal { get; set; }
    
    public bool? Sexo { get; set; }
    
    public bool? TipoPaciente { get; set; }
    
    public DateTime? FechaNacimiento { get; set; }
    
    public IFormFile Foto { get; set; }
}

public class ModificarPacienteHandler : IRequestHandler<ModificarPaciente>
{
    private readonly FisioContext _context;
    private readonly IConvertType _convertType;
    private readonly IExistResource _existResource;
    private readonly IPacienteValidator _validator;
    
    public ModificarPacienteHandler(FisioContext context, IConvertType convertType, IExistResource existResource, IPacienteValidator validator)
    {
        _context = context;
        _validator = validator;
        _convertType = convertType;
        _existResource = existResource;
    }
    
    public async Task Handle(ModificarPaciente request, CancellationToken cancellationToken)
    {
        // Validacion
        await _validator.modifyPatient(request);
        
        // Verificar si existe el estado civil y el fisioterapeuta
        if(request.EstadoCivilId != null)
            await _existResource.ExistEstadoCivil(request.EstadoCivilId);
        if(request.FisioterapeutaId != null)
            await _existResource.ExistFisioterapeuta(request.FisioterapeutaId);
        
        var paciente = await _context.Pacientes
            .FindAsync(request.PacienteId.HashIdInt())
            ?? throw new NotFoundException(Message.PACI_0016);
        
        paciente.Nombre = request.Nombre ?? paciente.Nombre;
        paciente.Apellido = request.Apellido ?? paciente.Apellido;
        paciente.Edad = request.FechaNacimiento ?? paciente.Edad;
        paciente.TipoPaciente = request.TipoPaciente ?? paciente.TipoPaciente;
        paciente.Notas = request.Notas ?? paciente.Notas;
        paciente.Ocupacion = request.Ocupacion ?? paciente.Ocupacion;
        paciente.Telefono = request.Telefono ?? paciente.Telefono;
        paciente.Institucion = request.Institucion ?? paciente.Institucion;
        paciente.Domicilio = request.Direccion ?? paciente.Domicilio;
        paciente.CodigoPostal = request.CodigoPostal ?? paciente.CodigoPostal;
        paciente.FisioterapeutaId = request.FisioterapeutaId == null ? paciente.FisioterapeutaId : request.FisioterapeutaId.HashIdInt();
        paciente.EstadoCivilId = request.EstadoCivilId == null ? paciente.EstadoCivilId : request.EstadoCivilId.HashIdInt();
        paciente.Sexo = request.Sexo ?? paciente.Sexo;
        paciente.Foto = request.Foto == null ? paciente.Foto : await _convertType.uploadFile(request.Foto);

        _context.Pacientes.Update(paciente);
        await _context.SaveChangesAsync();
    }
}
