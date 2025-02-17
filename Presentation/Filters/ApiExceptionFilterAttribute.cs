using Application.Core.Domain.Exceptions;
using Core.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
        }

        base.OnException(context);
    }

    private void HandleValidationException(ExceptionContext context, ValidationException exception)
    {
        ValidationProblemDetails details;

        if (exception.Errores is not null)
        {
            details = new ValidationProblemDetails(exception.Errores)
            {
                Title = "Uno o más errores de validación han ocurrido.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }
        else
        {
            details = new ValidationProblemDetails
            {
                Title = "Uno o más errores de validación han ocurrido.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Message,
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }
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
}