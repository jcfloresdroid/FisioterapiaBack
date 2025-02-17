using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;

namespace Core.Services.Implementacion;

public class ExistResource : IExistResource
{
    private readonly FisioContext _context;
    
    public ExistResource(FisioContext context)
    {
        _context = context;
    }
    
    public async Task ExistEspecialidad(string especialidadId)
    {
        Cat_Especialidades especialidad = await _context.Especialidades
            .FindAsync(especialidadId.HashIdInt()) ?? throw new NotFoundException(Message.CAT_0001);
    }

    public async Task ExistEstadoCivil(string estadoCivilId)
    {
        Cat_EstadoCivil estadoCivil = await _context.EstadoCivils
            .FindAsync(estadoCivilId.HashIdInt()) ?? throw new NotFoundException(Message.CAT_0002);
    }

    public async Task ExistServicio(string servicioId)
    {
        Cat_Servicios servicios = await _context.Servicios
            .FindAsync(servicioId.HashIdInt()) ?? throw new NotFoundException(Message.CAT_0005);
    }

    public async Task ExistMotivoAlta(string motivoAltaId)
    {
        Cat_MotivoAlta motivo = await _context.MotivoAltas
            .FindAsync(motivoAltaId.HashIdInt()) ?? throw new NotFoundException(Message.CAT_0004);
    }
    public async Task ExistPatologias(string PatologiasId)
    {
        Cat_Patologias patologia = await _context.Patologias
            .FindAsync(PatologiasId.HashIdInt()) ?? throw new NotFoundException(Message.CAT_0005);
    }

    public async Task ExistPaciente(string pacienteId)
    {
        Paciente paciente = await _context.Pacientes
            .FindAsync(pacienteId.HashIdInt()) ?? throw new NotFoundException(Message.PACI_0016);
    }

    public async Task ExistFisioterapeuta(string fisioId)
    {
        Fisioterapeuta fisio = await _context.Fisioterapeuta
            .FindAsync(fisioId.HashIdInt()) ?? throw new NotFoundException(Message.CAT_0008);
    }

    public async Task ExistExpediente(string expedienteId)
    {
        Expediente fisio = await _context.Expedientes
            .FindAsync(expedienteId.HashIdInt()) ?? throw new NotFoundException(Message.EXPE_0001);
    }

    public async Task ExistDiagnostico(string diagnosticoId)
    {
        Diagnostico diagnostico = await _context.Diagnosticos
            .FindAsync(diagnosticoId.HashIdInt()) ?? throw new NotFoundException(Message.DIAG_0001);
    }
}