using Application.Core.Domain.Exceptions;
using Core.Features.Diagnostico.command;
using Core.Services.Interfaz.Validator;
using Core.Validator.Diagnostico;

namespace Core.Services.Implementacion.Validator;

public class DiagnosticoValidator : IDiagnosticoValidator
{
    private readonly AddDiagnostico _validator;
    private readonly CrearRevisiones _revisionValidator;
    private readonly EndDiagnostico _endDiagnostico;
    
    public DiagnosticoValidator(AddDiagnostico validator, CrearRevisiones revisionValidator, EndDiagnostico endDiagnostico)
    {
        _validator = validator;
        _revisionValidator = revisionValidator;
        _endDiagnostico = endDiagnostico;
    }
    
    public async Task crearDiagnostico(GeneralDiagnosticPost diagnostico)
    {
        var diagValidator = await _validator.ValidateAsync(diagnostico);

        if (!diagValidator.IsValid)
            throw new ValidationException(diagValidator.Errors);
    }

    public async Task FinDiagnostico(FinalizarDiagnostico finalizar)
    {
        var finalizarValidator = await _endDiagnostico.ValidateAsync(finalizar);

        if (!finalizarValidator.IsValid)
            throw new ValidationException(finalizarValidator.Errors);
    }

    public async Task AddRevision(PostRevisiones revision)
    {
        var revisionValidator = await _revisionValidator.ValidateAsync(revision);

        if (!revisionValidator.IsValid)
            throw new ValidationException(revisionValidator.Errors);
    }
}