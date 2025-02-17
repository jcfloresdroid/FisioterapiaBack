using Core.Features.Cuenta.command;
using FluentValidation;

namespace Core.Validator.Cuenta;

public class ContraseñaValidator : AbstractValidator<ModificarContraseñas>
{
    public ContraseñaValidator()
    {
        RuleFor(x => x.ContraseñaActual)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.MPASS_0001);

        RuleFor(x => x.NuevaContraseña)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.MPASS_0002);

        RuleFor(x => x.ConfirmarContraseña)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.MPASS_0003);
    }
}