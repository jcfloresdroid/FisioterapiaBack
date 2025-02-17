using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;

namespace Core.Features.Diagnostico.command;

public record ExploracionReview(){
    public int Fr { get; set; }
    public int Fc { get; set; }
    public float Temperatura { get; set; }
    public float Peso { get; set; }
    public float Estatura { get; set; }
    public float Imc { get; set; }
    public float IndiceCinturaCadera { get; set; }
    public float SaturacionOxigeno { get; set; }
    public string PresionArterial { get; set; }
}

public record PostRevisiones() : IRequest
{
    public string Notas { get; set; }
    public string FolioPago { get; set; }
    public string DiagnosticoId { get; set; }
    public string ServicioId { get; set; }
    public ExploracionReview Exploration { get; set; }
}

public class PostRevisionHandler : IRequestHandler<PostRevisiones>
{
    private readonly FisioContext _context;
    private readonly IDiagnosticoValidator _validator;
    private readonly IExistResource _existResource;
    
    public PostRevisionHandler(FisioContext context, IDiagnosticoValidator validator, IExistResource existResource)
    {
        _context = context;
        _validator = validator;
        _existResource = existResource;
    }
    
    public async Task Handle(PostRevisiones request, CancellationToken cancellationToken)
    {
        // Validaciones
        await _validator.AddRevision(request);
        await _existResource.ExistServicio(request.ServicioId);
        await _existResource.ExistDiagnostico(request.DiagnosticoId);
        
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                /* Creamos la exploracion Fisica */
                var exploracion = new ExploracionFisica()
                {
                    Fr = request.Exploration.Fr,
                    Fc = request.Exploration.Fc,
                    Temperatura = request.Exploration.Temperatura,
                    Peso = request.Exploration.Peso,
                    Estatura = request.Exploration.Estatura,
                    Imc = request.Exploration.Imc,
                    IndiceCinturaCadera = request.Exploration.IndiceCinturaCadera,
                    SaturacionOxigeno = request.Exploration.SaturacionOxigeno,
                    PresionArterial = request.Exploration.PresionArterial
                };
                
                await _context.ExploracionFisicas.AddAsync(exploracion);
                await _context.SaveChangesAsync();
                
                var revision = new Revision
                {
                    Notas = request.Notas,
                    FolioPago = request.FolioPago,
                    Fecha = FormatDate.DateLocal(),
                    Hora = new TimeSpan(FormatDate.DateLocal().Hour, FormatDate.DateLocal().Minute, 0),
                    DiagnosticoId = request.DiagnosticoId.HashIdInt(),
                    ExploracionFisicaId = exploracion.ExploracionFisicaId,
                    ServicioId = request.ServicioId.HashIdInt()
                };
        
                await _context.Revisions.AddAsync(revision);
                await _context.SaveChangesAsync();
                
                //Si todo sale bien commitiar 
                transaction.Commit();
            }
            catch (Exception e)
            {
                // Si ocurre un error, revertir la transacción
                try
                {
                    transaction.Rollback();
                }
                catch (Exception exRollback)
                {
                    Console.WriteLine("Error al revertir la transacción: " + exRollback.Message);
                }

                throw new BadRequestException("Error al procesar los datos");
            }
        }
    }
}