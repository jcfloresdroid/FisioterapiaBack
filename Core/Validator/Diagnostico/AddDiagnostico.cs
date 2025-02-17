using Core.Features.Diagnostico.command;
using FluentValidation;

namespace Core.Validator.Diagnostico;

public class AddDiagnostico : AbstractValidator<GeneralDiagnosticPost>
{
    public AddDiagnostico()
    {
        RuleFor(x => x.ExpedienteId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.EXPE_0001);
        
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

        // Mapa
        RuleFor(x => x.Map.valores).NotNull().WithMessage(Message.MAP_0002);
        RuleFor(x => x.Map.RangoDolor).NotNull().WithMessage(Message.MAP_0003);
        RuleFor(x => x.Map.Nota).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.MAP_0004);
        
        // Diagnostico
        RuleFor(x => x.Diagnostic.Descripcion).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0002);
        RuleFor(x => x.Diagnostic.Refiere).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0003);
        RuleFor(x => x.Diagnostic.Categoria).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0004);
        RuleFor(x => x.Diagnostic.DiagnosticoPrevio).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0005);
        RuleFor(x => x.Diagnostic.TerapeuticaEmpleada).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0006);
        RuleFor(x => x.Diagnostic.DiagnosticoFuncional).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0007);
        RuleFor(x => x.Diagnostic.PadecimientoActual).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0008);
        RuleFor(x => x.Diagnostic.Inspeccion).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0009);
        RuleFor(x => x.Diagnostic.ExploracionFisicaDescripcion).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0010);
        RuleFor(x => x.Diagnostic.EstudiosComplementarios).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0011);
        RuleFor(x => x.Diagnostic.DiagnosticoNosologico).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.DIAG_0012);
        
        // Programa
        RuleFor(x => x.Program.CortoPlazo).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0002);
        RuleFor(x => x.Program.MedianoPlazo).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0003);
        RuleFor(x => x.Program.LargoPlazo).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0004);
        RuleFor(x => x.Program.TratamientoFisioterapeutico).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0005);
        RuleFor(x => x.Program.Sugerencias).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0006);
        RuleFor(x => x.Program.Pronostico).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.PROG_0007);
        
        // Revision
        RuleFor(x => x.Review.Notas).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.REV_0002);
        RuleFor(x => x.Review.FolioPago).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.REV_0003);
        RuleFor(x => x.Review.ServicioId).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(Message.REV_0004);
    }
}