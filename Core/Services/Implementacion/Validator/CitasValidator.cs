using Application.Core.Domain.Exceptions;
using Core.Features.Citas.command;
using Core.Services.Interfaz.Validator;
using Core.Validator.Citas;

namespace Core.Services.Implementacion.Validator;

public class CitasValidator : ICitasValidator
{
    private readonly AddCita _validator;
    
    public CitasValidator()
    {
        _validator = new AddCita();
    }
    
    public async Task AgregarCita(PostDate cita)
    {
        var citaValidator = await _validator.ValidateAsync(cita);

        if (!citaValidator.IsValid)
            throw new ValidationException(citaValidator.Errors);
    }
}