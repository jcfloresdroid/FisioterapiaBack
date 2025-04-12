using Core.Features.Diagnostico.command;
using FluentValidation;

namespace Core.Validator.Diagnostico;

public class ModifyDiagnostico : AbstractValidator<EditDiagnostico>
{
    public ModifyDiagnostico()
    {
        RuleFor(x => x.Descripcion).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0002);
        RuleFor(x => x.Refiere).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0003);
        RuleFor(x => x.Categoria).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0004);
        RuleFor(x => x.DiagnosticoPrevio).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0005);
        RuleFor(x => x.TerapeuticaEmpleada).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0006);
        RuleFor(x => x.DiagnosticoFuncional).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0007);
        RuleFor(x => x.PadecimientoActual).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0008);
        RuleFor(x => x.Inspeccion).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0009);
        RuleFor(x => x.ExploracionFisicaDescripcion).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0010);
        RuleFor(x => x.EstudiosComplementarios).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0011);
        RuleFor(x => x.DiagnosticoNosologico).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0012);
        
        
        
        
        RuleFor(x => x.CortoPlazo).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0002);
        RuleFor(x => x.MedianoPlazo).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0003);
        RuleFor(x => x.LargoPlazo).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0004);
        RuleFor(x => x.TratamientoFisioterapeutico).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0005);
        RuleFor(x => x.Sugerencias).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0006);
        RuleFor(x => x.Pronostico).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0007);
    }
}