using Core.Features.Pacientes.Command;
using FluentValidation;

namespace Core.Validator.Expediente;

public class GenerarExpediente : AbstractValidator<PostExpedient>
{
    public GenerarExpediente()
    {
        RuleFor(x => x.PacienteId).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PACI_0001);
        RuleFor(x => x.TipoInterrogatorio)
            .Must(x => x == true || x == false)
            .WithMessage(Message.EXPE_0002);
        
        RuleFor(x => x.HeredoFamiliar.Padres).NotNull().WithMessage(Message.HERE_0001);
        RuleFor(x => x.HeredoFamiliar.PadresVivos).NotNull().WithMessage(Message.HERE_0002);
        RuleFor(x => x.HeredoFamiliar.Hermanos).NotNull().WithMessage(Message.HERE_0003);
        RuleFor(x => x.HeredoFamiliar.HermanosVivos).NotNull().WithMessage(Message.HERE_0004);
        RuleFor(x => x.HeredoFamiliar.Hijos).NotNull().WithMessage(Message.HERE_0005);
        RuleFor(x => x.HeredoFamiliar.HijosVivos).NotNull().WithMessage(Message.HERE_0006);
        
        RuleFor(x => x.Antecedente.AntecedentesPatologicos).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.ANTE_0001);
        RuleFor(x => x.Antecedente.MedioLaboral).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.ANTE_0002);
        RuleFor(x => x.Antecedente.MedioSociocultural).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.ANTE_0003);
        RuleFor(x => x.Antecedente.MedioFisicoambiental).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.ANTE_0004);
    }
}