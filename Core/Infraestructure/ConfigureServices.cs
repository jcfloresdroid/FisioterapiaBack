using System.Text;
using Core.Domain.Helpers;
using Core.Domain.Validators;
using Core.Services.Implementacion;
using Core.Services.Implementacion.Validator;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Core.Infraestructure;

public static class ConfigureServices
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
    {
        HashConvert.Configure(config);
        
        #region Inyeccion de la Intefaz con su implementacion
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDate, Date>();
        services.AddScoped<IUtilPdf, UtilPdf>();
        services.AddScoped<IConvertType, ConvertType>();
        services.AddScoped<IExistResource, ExistResource>();
        services.AddScoped<ICitasValidator, CitasValidator>();
        services.AddScoped<ICuentaValidator, CuentaValidator>();
        services.AddScoped<IDiagnosticoValidator, DiagnosticoValidator>();
        services.AddScoped<IExpedienteValidator, ExpedienteValidator>();
        services.AddScoped<IFisioValidator, FisioValidator>();
        services.AddScoped<IPacienteValidator, PacienteValidator>();
        services.AddScoped<IUsuarioValidator, UsuarioValidator>();
        #endregion

        #region Fluent Validation
        services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginValidator>());
        #endregion
        
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        
        return services;
    }
}