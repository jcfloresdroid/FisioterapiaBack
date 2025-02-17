using Core.Features.Fisioterapeutas.command;
using FluentValidation;

namespace Core.Validator.Paciente;

public class PatientEstatus : AbstractValidator<StatusPaciente>
{
    public PatientEstatus()
    {
        RuleFor(x => x.PacienteId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0001);
    }
}