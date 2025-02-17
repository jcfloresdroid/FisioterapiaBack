using Core.Features.Diagnostico.command;
using FluentValidation;

namespace Core.Validator.Diagnostico;

public class EndDiagnostico : AbstractValidator<FinalizarDiagnostico>
{
    public EndDiagnostico()
    {
        RuleFor(x => x.DiagnosticId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.DIAG_0015);

        RuleFor(x => x.DiagnosticoInicial)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.DIAG_0016);

        RuleFor(x => x.DiagnosticoFinal)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.DIAG_0017);

        RuleFor(x => x.FrecuenciaTratamiento)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.DIAG_0018);

        RuleFor(x => x.MotivoAltaId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.DIAG_0019);
    }
}