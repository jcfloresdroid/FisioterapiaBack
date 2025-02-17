using Core.Features.Cuenta.command;
using FluentValidation;

namespace Core.Validator.Cuenta;

public class ClaveValidator : AbstractValidator<ModificarClave>
{
    public ClaveValidator()
    {
        RuleFor(x => x.ClaveActual)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.MCLAVE_0001);
        
        RuleFor(x => x.ClaveNueva)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.MCLAVE_0002);
        
        RuleFor(x => x.ConfirmarClave)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.MCLAVE_0003);
    }
}