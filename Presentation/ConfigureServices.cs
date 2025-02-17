using System.Reflection;
using Core.Services.Interfaz;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Presentation.Filters;
using Authorization = Presentation.Services.Authorization;

namespace Presentation;

public static class ConfigureServices
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration config)
    {
        //Configura la manera en que se envian los errores, Son para los modelos
        /*services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new ErrorResponse
                    {
                        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                        Title = "Los datos ingresados son incorrectos",
                        Status = (int)HttpStatusCode.BadRequest,
                        Detail = e.Value.Errors.First().ErrorMessage
                    }).ToList();

                return new BadRequestObjectResult(errors);
            };
        });*/
        
        services.Configure<ApiBehaviorOptions>(options => {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo {
                Title = "Fisiolabs_Software", 
                Version = "v1" 
            });
    
            c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme {
                Name = "Authorization",
                Description = "Porfavor inserta tu JWT Bearer en este campo",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
            
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // Genera los documentos
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Presentation.xml"));
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Core.xml"));
        });

        // Agrega el HTTPContext y este sirve para poder obtener el usuario
        services.AddHttpContextAccessor();
        services.AddTransient<IAuthorization, Authorization>();

        // Genera las excepciones
        services.AddControllers(options => 
            options.Filters.Add<ApiExceptionFilterAttribute>()).AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter())
        );

        return services;
    }
}