using Application.Core.Domain.Exceptions;
using Core.Features.Pacientes.Command;
using Core.Services.Interfaz.Validator;
using Core.Validator.Expediente;

namespace Core.Services.Implementacion.Validator;

public class ExpedienteValidator : IExpedienteValidator
{
    private readonly GenerarExpediente _expedienteValidator;
    
    public ExpedienteValidator()
    {
        _expedienteValidator = new GenerarExpediente();
    }
    
    public async Task addExpediente(PostExpedient expediente)
    {
        var expValidator = await _expedienteValidator.ValidateAsync(expediente);

        if (!expValidator.IsValid)
            throw new ValidationException(expValidator.Errors);
    }
}