using Core.Features.Fisioterapeutas.command;
using FluentValidation;

namespace Core.Validator.Fisio;

public class AddFisio : AbstractValidator<PostFisioterapeutas>
{
    public AddFisio()
    {
        RuleFor(x => x.Nombre)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.FIS_0002);
        
        RuleFor(x => x.Correo)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.FIS_0003)
            .EmailAddress()
            .WithMessage(Message.FIS_0004);
        
        RuleFor(x => x.Telefono)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.FIS_0005)
            .Matches(@"^\d{10}$")
            .WithMessage(Message.FIS_0006);
        
        RuleFor(x => x.EspecialidadId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.FIS_0007);
    }
}