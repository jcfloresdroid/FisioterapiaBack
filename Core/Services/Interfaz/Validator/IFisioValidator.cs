using Core.Features.Fisioterapeutas.command;

namespace Core.Services.Interfaz.Validator;

public interface IFisioValidator
{
    Task CreateFisio(PostFisioterapeutas fisio);
}