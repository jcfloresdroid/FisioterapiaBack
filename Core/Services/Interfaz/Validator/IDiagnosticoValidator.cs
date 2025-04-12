using Core.Features.Diagnostico.command;

namespace Core.Services.Interfaz.Validator;

public interface IDiagnosticoValidator
{
    Task crearDiagnostico(GeneralDiagnosticPost diagnostico);
    Task ModificarDiagnostico(EditDiagnostico diagnostico);
    Task FinDiagnostico(FinalizarDiagnostico finalizar);
    Task AddRevision(PostRevisiones revision);
}