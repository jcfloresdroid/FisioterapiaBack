using Core.Features.Cuenta.command;
using FluentValidation;

namespace Core.Domain.Validators;
 
public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x => new { x.Username, x.Contraseña })
            .Must(x => !string.IsNullOrWhiteSpace(x.Username) && !string.IsNullOrWhiteSpace(x.Contraseña))
            .WithMessage(Message.LOGIN_00001);
        
        /*RuleFor(r => r.Username)
            .NotNull()
            .NotEmpty()
                .WithMessage("El campo nombre no puede ser nulo");
        
        RuleFor(r => r.Contraseña)
            .NotNull()
            .NotEmpty()
            .WithMessage("El campo contraseña no puede ser nulo");*/
    }
}