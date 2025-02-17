using Core.Features.Fisioterapeutas.command;
using Core.Features.Pacientes.command;
using Core.Features.Pacientes.Command;

namespace Core.Services.Interfaz.Validator;

public interface IPacienteValidator
{
    Task addPatient(CreatePatient patient);
    Task modifyPatient(ModificarPaciente patient);
    Task estadoPatient(StatusPaciente patient);
}