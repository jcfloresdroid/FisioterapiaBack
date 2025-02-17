using Core.Features.Pacientes.Command;

namespace Core.Services.Interfaz.Validator;

public interface IExpedienteValidator
{
    Task addExpediente(PostExpedient expediente);
}