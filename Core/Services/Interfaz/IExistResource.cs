using Core.Domain.Entities;

namespace Core.Services.Interfaz;

public interface IExistResource
{
    Task ExistEspecialidad(string especialidadId);
    Task ExistEstadoCivil(string estadoCivilId);
    Task ExistServicio(string servicioId);
    Task ExistMotivoAlta(string motivoAltaId);
    Task ExistPaciente(string pacienteId);
    Task ExistFisioterapeuta(string fisioId);
    Task ExistExpediente(string expedienteId);
    Task ExistDiagnostico(string diagnosticoId);
}