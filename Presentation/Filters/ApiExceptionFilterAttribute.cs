using System.Text.RegularExpressions;
using Application.Core.Domain.Exceptions;
using Core.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Presentation.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public readonly ILogger<ApiExceptionFilterAttribute> _Logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _Logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException validationEx:
                HandleValidationException(context, validationEx);
                _Logger.LogDebug(context.Exception, "{message} en {@Result}", context.Exception.Message,
                    context.Result);
                break;
            case NotFoundException notFoundException:
                HandleNotFoundException(context, notFoundException);
                _Logger.LogDebug(context.Exception, "{message} en {@Result}", context.Exception.Message,
                    context.Result);
                break;
            case ConflictException conflictException:
                HandleConflictException(context, conflictException);
                _Logger.LogDebug(context.Exception, "{message} en {@Result}", context.Exception.Message,
                    context.Result);
                break;
            case BadRequestException badRequestException:
                HandleBadRequestException(context, badRequestException);
                _Logger.LogDebug(context.Exception, "{message} en {@Result}", context.Exception.Message,
                    context.Result);
                break;
            case UnauthorizedException unhauthorizedException:
                HandleUnauthorizedException(context, unhauthorizedException);
                _Logger.LogDebug(context.Exception, "{message} en {@Result}", context.Exception.Message,
                    context.Result);
                break;
            case ForbiddenAccessException:
                HandleForbiddenAccessException(context);
                _Logger.LogDebug(context.Exception, "{message} en {@Result}", context.Exception.Message,
                    context.Result);
                break;
            
            case DbUpdateException dbUpdateException:
                HandleDbUpdateException(context, dbUpdateException);
                _Logger.LogDebug(context.Exception, "{message} en {@Result}", context.Exception.Message,
                    context.Result);
                break;
            
            default:
                HandleUnknownException(context);
                _Logger.LogError(context.Exception, "{message}", context.Exception.Message);
                break;
        }

        base.OnException(context);
    }

    private void HandleValidationException(ExceptionContext context, ValidationException exception)
    {
        if (exception.Error is not null)
        {
            var details = new ProblemDetails
            {
                Title = "Error de validación",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Error,
                Status = StatusCodes.Status400BadRequest
            };

            context.Result = new BadRequestObjectResult(details);
        }
        else
        {
            var details = new ProblemDetails
            {
                Title = "Uno o más errores de validación han ocurrido.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Result = new BadRequestObjectResult(details);
        }

        context.ExceptionHandled = true;
    }
    
    private void HandleNotFoundException(ExceptionContext context, NotFoundException exception)
    {
        var details = new ProblemDetails()
        {
            Title = "No se ha encontrado el recurso especificado",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleConflictException(ExceptionContext context, ConflictException exception)
    {
        var details = new ProblemDetails()
        {
            Title = "Error de duplicidad",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            Detail = exception.Message
        };

        context.Result = new ConflictObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleBadRequestException(ExceptionContext context, BadRequestException exception)
    {
        var details = new ProblemDetails()
        {
            Title = "Los datos ingresados son incorrectos",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Detail = exception.Message
        };
    
        context.Result = new BadRequestObjectResult(details);
    
        context.ExceptionHandled = true;
    }
    
    private void HandleUnauthorizedException(ExceptionContext context, UnauthorizedException exception)
    {
        var details = new ProblemDetails()
        {
            Title = "No autorizado",
            Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            Detail = exception.Message
        };
    
        context.Result = new UnauthorizedObjectResult(details);
    
        context.ExceptionHandled = true;
    }
    
    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Acceso denegado.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        context.ExceptionHandled = true;
    }
    
    private void HandleDbUpdateException(ExceptionContext context, DbUpdateException exception)
    {
        // Verificar si es un error de clave única el 1062 es el código de error de MySQL
        if (exception.InnerException is MySqlException sqlException && sqlException.Number == 1062)
        {
            var match = Regex.Match(sqlException.Message, @"for key '(.*?)'"); // Extrae el nombre del índice

            string field = "desconocido";
            if (match.Success)
            {
                string fullKey = match.Groups[1].Value;
                string[] parts = fullKey.Split('_'); // Divide el índice en partes por el carácter '_'
                field = parts.Last(); // Toma la última parte del arreglo
            }

            var details = new ProblemDetails
            {
                Title = "Violación de restricción de clave única",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"El dato ingresado en el campo {field} ya existe."
            };

            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }
        else
        {
            // Manejar otros errores de DbUpdateException
            HandleUnknownException(context);
        }
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error interno del servidor",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "Ha ocurrido un error al procesar la solicitud."
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}