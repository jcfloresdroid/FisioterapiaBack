using Core.Features.Citas.command;

namespace Core.Services.Interfaz.Validator;

public interface ICitasValidator
{
    Task AgregarCita(PostDate cita);
}