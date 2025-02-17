using Core.Features.Citas.command;
using FluentValidation;

namespace Core.Validator.Citas;

public class AddCita : AbstractValidator<PostDate>
{
    public AddCita()
    {
        RuleFor(x => x.PacienteId)
            .NotNull()
            .WithMessage(Message.PACI_0001);

        RuleFor(x => x.Fecha)
            .NotNull()
            .WithMessage(Message.CITA_0001);

        RuleFor(x => x.Hora)
            .NotNull()
            .WithMessage(Message.CITA_0002);
        
        RuleFor(x => x.Motivo)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.CITA_0003);
    }
}