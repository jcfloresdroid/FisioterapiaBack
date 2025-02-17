using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Presentation.Models;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext) {
        try {
            await _next(httpContext);
        }
        catch (Exception ex) {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception) {
        context.Response.ContentType = "application/json";

        if (exception is DbUpdateException dbUpdateException) {
            // Verificar si es un error de clave única el 1062 es el código de error de MySQL
            if (dbUpdateException.InnerException is MySqlException sqlException && sqlException.Number == 1062) { 
                var match = Regex.Match(sqlException.Message, @"for key '(.*?)'"); // Extrae el nombre del índice

                string field = "desconocido";
                if (match.Success) {
                    string fullKey = match.Groups[1].Value;
                    string[] parts = fullKey.Split('_'); // Divide el índice en partes por el carácter '_'
                    field = parts.Last(); // Toma la última parte del arreglo
                }
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    Title = "Violación de restricción de clave única",
                    Status = context.Response.StatusCode,
                    Detail = $"El dato ingresado en el campo {field} ya existe."
                }));
            }
        }
        
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "Error Interno del Servidor",
            Status = context.Response.StatusCode,
            Detail = exception.Message
        }));
    }
}
