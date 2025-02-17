using Core.Features.Diagnostico.command;
using FluentValidation;

namespace Core.Validator.Diagnostico;

public class CrearRevisiones : AbstractValidator<PostRevisiones>
{
    public CrearRevisiones()
    {
        // Revision
        RuleFor(x => x.Notas).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.REV_0002);
        RuleFor(x => x.FolioPago).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.REV_0003);
        RuleFor(x => x.ServicioId).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.REV_0004);
        RuleFor(x => x.DiagnosticoId).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0015);

        
        // Exploracion
        RuleFor(x => x.Exploration.Fr).NotNull().WithMessage(Message.EXP_0002);
        RuleFor(x => x.Exploration.Fc).NotNull().WithMessage(Message.EXP_0003);
        RuleFor(x => x.Exploration.Temperatura).NotNull().WithMessage(Message.EXP_0004);
        RuleFor(x => x.Exploration.Peso).NotNull().WithMessage(Message.EXP_0005);
        RuleFor(x => x.Exploration.Estatura).NotNull().WithMessage(Message.EXP_0006);
        RuleFor(x => x.Exploration.Imc).NotNull().WithMessage(Message.EXP_0007);
        RuleFor(x => x.Exploration.IndiceCinturaCadera).NotNull().WithMessage(Message.EXP_0008);
        RuleFor(x => x.Exploration.SaturacionOxigeno).NotNull().WithMessage(Message.EXP_0009);
        RuleFor(x => x.Exploration.PresionArterial).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.EXP_0010);
    }
}