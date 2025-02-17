using Core.Features.Pacientes.Command;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Core.Validator.Paciente;

public class AddPatient : AbstractValidator<CreatePatient>
{
    public AddPatient()
    {
        RuleFor(x => x.Nombre)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0002);
        
        RuleFor(x => x.Apellido)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0003);
        
        RuleFor(x => x.Institucion)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0004);
        
        RuleFor(x => x.Domicilio)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0005);
        
        RuleFor(x => x.Ocupacion)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0006);
        
        RuleFor(x => x.Telefono)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0007)
            .Matches(@"^\d{10}$")
            .WithMessage(Message.PACI_0008);

        RuleFor(x => x.Sexo)
            .NotNull()
            .WithMessage(Message.PACI_0009);
        
        RuleFor(x => x.TipoPaciente)
            .NotNull()
            .WithMessage(Message.PACI_0010);
        
        RuleFor(x => x.Edad)
            .NotNull()
            .WithMessage(Message.PACI_0011);

        RuleFor(x => x.CodigoPostal)
            .NotNull()
            .WithMessage(Message.PACI_0012);
        
        RuleFor(x => x.EstadoCivilId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0013);
        
        RuleFor(x => x.FisioterapeutaId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.PACI_0014);
    }
    
    private bool ValidarFormato(IFormFile file)
    {
        var formatos = new[] { "image/jpeg", "image/png" };
        return file != null && formatos.Contains(file.ContentType);
    }
}