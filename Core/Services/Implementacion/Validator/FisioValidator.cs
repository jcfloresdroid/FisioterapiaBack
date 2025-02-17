using Application.Core.Domain.Exceptions;
using Core.Features.Fisioterapeutas.command;
using Core.Services.Interfaz.Validator;
using Core.Validator.Fisio;

namespace Core.Services.Implementacion.Validator;

public class FisioValidator : IFisioValidator
{
    private readonly AddFisio _validator;
    
    public FisioValidator(AddFisio validator)
    {
        _validator = validator;
    }
    
    public async Task CreateFisio(PostFisioterapeutas fisio)
    {
        var fisioValidator = await _validator.ValidateAsync(fisio);

        if (!fisioValidator.IsValid)
            throw new ValidationException(fisioValidator.Errors);
    }
}