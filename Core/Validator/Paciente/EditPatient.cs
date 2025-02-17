using Core.Features.Pacientes.command;
using FluentValidation;

namespace Core.Validator.Paciente;

public class EditPatient : AbstractValidator<ModificarPaciente>
{
    public EditPatient()
    {
        RuleFor(x => x.Telefono)
            .Matches(@"^\d{10}$")
            .WithMessage(Message.PACI_0008);
    }
}