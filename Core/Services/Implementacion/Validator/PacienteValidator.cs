using Application.Core.Domain.Exceptions;
using Core.Features.Fisioterapeutas.command;
using Core.Features.Pacientes.command;
using Core.Features.Pacientes.Command;
using Core.Services.Interfaz.Validator;
using Core.Validator.Paciente;

namespace Core.Services.Implementacion.Validator;

public class PacienteValidator : IPacienteValidator
{
    private readonly AddPatient _validator;
    private readonly EditPatient _editPatient;
    private readonly PatientEstatus _estatus;
    
    public PacienteValidator(AddPatient validator, EditPatient editPatient, PatientEstatus estatus)
    {
        _validator = validator;
        _editPatient = editPatient;
        _estatus = estatus;
    }

    public async Task addPatient(CreatePatient patient)
    {
        var pacienteValidator = await _validator.ValidateAsync(patient);

        if (!pacienteValidator.IsValid)
            throw new ValidationException(pacienteValidator.Errors);
    }

    public async Task modifyPatient(ModificarPaciente patient)
    {
        var pacienteValidator = await _editPatient.ValidateAsync(patient);

        if (!pacienteValidator.IsValid)
            throw new ValidationException(pacienteValidator.Errors);
    }

    public async Task estadoPatient(StatusPaciente patient)
    {
        var pacienteValidator = await _estatus.ValidateAsync(patient);

        if (!pacienteValidator.IsValid)
            throw new ValidationException(pacienteValidator.Errors);
    }
}