using Core.Features.Usuario.command;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Core.Validator.Usuario;

public class FotoMValidator : AbstractValidator<ModificarFoto>
{
    public FotoMValidator()
    {
        RuleFor(x => x.Foto)
            .NotNull()
            .WithMessage(Message.USER_00007)
            .DependentRules(() =>
            {
                RuleFor(x => x.Foto)
                    .Must(file => ValidarFormato(file))
                    .WithMessage(Message.USER_00008)
                    .Must(file => file.Length > 0)
                    .WithMessage(Message.USER_00009)
                    .Must(file => file.Length <= 1 * 1024 * 1024)
                    .WithMessage(Message.USER_00010);
            });
    }

    private bool ValidarFormato(IFormFile file)
    {
        var formatos = new[] { "image/jpeg", "image/png" };
        return file != null && formatos.Contains(file.ContentType);
    }
}