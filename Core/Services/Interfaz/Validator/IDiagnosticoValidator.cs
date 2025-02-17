using Core.Features.Diagnostico.command;

namespace Core.Services.Interfaz.Validator;

public interface IDiagnosticoValidator
{
    Task crearDiagnostico(GeneralDiagnosticPost diagnostico);
    Task FinDiagnostico(FinalizarDiagnostico finalizar);
    Task AddRevision(PostRevisiones revision);
}