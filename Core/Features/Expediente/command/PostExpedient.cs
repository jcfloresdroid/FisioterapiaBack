using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Pacientes.Command;

public record FamilyHistoryPost
{
    private int _padres;
    public int Padres
    {
        get { return _padres; }
        set {
            if (value < 0) 
                _padres = 0;
            else if (value > 2)
                _padres = 2;
            else
                _padres = value;
        }
    }
    private int _padresVivos;
    public int PadresVivos
    {
        get { return _padresVivos; }
        set {
            if (value < 0) 
                _padresVivos = 0;
            else if (value > 2) 
                _padresVivos = 2;
            else if(_padresVivos > _padres)
                _padresVivos = _padres;
            else 
                _padresVivos = value;
        }
    }
    public string? PadresCausaMuerte { get; set; }
    private int _hermanos;
    public int Hermanos
    {
        get { return _hermanos; }
        set {
            if (value < 0) 
                _hermanos = 0;
            else 
                _hermanos = value;
        }
    }
    private int _hermanosVivos;
    public int HermanosVivos { 
        get { return _hermanosVivos; }
        set {
            if (value < 0) 
                _hermanosVivos = 0;
            else if(_hermanosVivos > _hermanos)
                _hermanosVivos = _hermanos;
            else 
                _hermanosVivos = value;
        } 
    }
    public string? HermanosCausaMuerte { get; set; }
    private int _hijos; 
    public int Hijos
    {
        get { return _hijos; }
        set {
            if (value < 0) 
                _hijos = 0;
            else 
                _hijos = value;
        }
    }
    private int _hijosVivos;
    public int HijosVivos
    {
        get { return _hijosVivos; }
        set {
            if (value < 0) 
                _hijosVivos = 0;
            else if(_hijosVivos > _hijos)
                _hijosVivos = _hijos;
            else 
                _hijosVivos = value;
        } 
    }
    public string? HijosCausaMuerte { get; set; }
    public string? Dm { get; set; }
    public string? Hta { get; set; }
    public string? Cancer { get; set; }
    public string? Alcoholismo { get; set; }
    public string? Tabaquismo { get; set; }
    public string? Drogas { get; set; }
}

public record AntecedentsPost
{
    public string AntecedentesPatologicos { get; set; }
    public string MedioLaboral { get; set; }
    public string MedioSociocultural { get; set; }
    public string MedioFisicoambiental { get; set; }
}

public record GinecobstetricoPost
{
    public string? Fum { get; set; }
    public string? Fpp { get; set; }
    public string? Menarca { get; set; }
    public string? Ritmo { get; set; }    
    public string? Cirugias { get; set; }
    public int? EdadGestional { get; set; }    
    public int? Semanas { get; set; }    
    public int? Gestas { get; set; }
    public int? Partos { get; set; }
    public int? Cesareas { get; set; }
    public int? Abortos { get; set; }
    public string FlujoVaginalId { get; set; }
    public string TipoAnticonceptivoId { get; set; }
}

public record PostExpedient : IRequest
{
    public string PacienteId { get; set; }
    public bool TipoInterrogatorio { get; set; }
    public string? Responsable { get; set; }
    public FamilyHistoryPost HeredoFamiliar { get; set; }
    public AntecedentsPost Antecedente { get; set; }
    public GinecobstetricoPost? Ginecobstetricos { get; set; }
};

public class PostExpedientHandler : IRequestHandler<PostExpedient>
{
    private readonly FisioContext _context;
    private readonly IExpedienteValidator _validator;

    public PostExpedientHandler(FisioContext context, IExpedienteValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task Handle(PostExpedient request, CancellationToken cancellationToken)
    {
        // Validaciones
        await _validator.addExpediente(request);
        
        //Buscamos si el paciente es hombre o mujer
        var paciente = await _context.Pacientes
            .Include(x => x.Expediente) 
            .FirstOrDefaultAsync(x => x.PacienteId == request.PacienteId.HashIdInt());
        
        if (paciente == null)
            throw new NotFoundException("El paciente con ese Id no existe");
        
        if(paciente.Expediente != null)
            throw new BadRequestException("El paciente ya cuenta con un expediente");
        
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                /* Creamos los datos no Patologicos y se guardan */
                var noPatologico = new NoPatologico()
                {
                    MedioLaboral = request.Antecedente.MedioLaboral,
                    MedioSociocultural = request.Antecedente.MedioSociocultural,
                    MedioFisicoambiental = request.Antecedente.MedioFisicoambiental
                };

                await _context.NoPatologicos.AddAsync(noPatologico);
                await _context.SaveChangesAsync();

                /* Creamos los datos de HeredoFamiliar y se guardan */
                var heredoFamiliar = new HeredoFamiliar()
                {
                    Padres = request.HeredoFamiliar.Padres,
                    PadresVivos = request.HeredoFamiliar.PadresVivos,
                    PadresCausaMuerte = request.HeredoFamiliar.PadresCausaMuerte ?? "Sin registro",
                    Hermanos = request.HeredoFamiliar.Hermanos,
                    HermanosVivos = request.HeredoFamiliar.HermanosVivos,
                    HermanosCausaMuerte = request.HeredoFamiliar.HermanosCausaMuerte ?? "Sin registro",
                    Hijos = request.HeredoFamiliar.Hijos,
                    HijosVivos = request.HeredoFamiliar.HijosVivos,
                    HijosCausaMuerte = request.HeredoFamiliar.HijosCausaMuerte ?? "Sin registro",
                    Dm = request.HeredoFamiliar.Dm ?? "Sin registro",
                    Hta = request.HeredoFamiliar.Hta ?? "Sin registro",
                    Cancer = request.HeredoFamiliar.Cancer ?? "Sin registro",
                    Alcoholismo = request.HeredoFamiliar.Alcoholismo ?? "Sin registro",
                    Tabaquismo = request.HeredoFamiliar.Tabaquismo ?? "Sin registro",
                    Drogas = request.HeredoFamiliar.Drogas ?? "Sin registro"
                };
                
                await _context.HeredoFamiliars.AddAsync(heredoFamiliar);
                await _context.SaveChangesAsync();
                
                /* Creamos el expediente del usuario */
                var expedient = new Expediente()
                {
                    TipoInterrogatorio = request.TipoInterrogatorio,
                    Nomenclatura = CodigoExpediente(request.PacienteId.HashIdInt()),
                    Responsable = request.Responsable ?? "Sin registro",
                    AntecedentesPatologicos = request.Antecedente.AntecedentesPatologicos,
                    PacienteId = request.PacienteId.HashIdInt(),
                    HeredoFamiliarId = heredoFamiliar.HeredoFamiliarId,
                    NoPatologicoId = noPatologico.NoPatologicoId
                };

                await _context.Expedientes.AddAsync(expedient);
                await _context.SaveChangesAsync();
                
                //Si el paciente es mujer se crea el ginecobstetrico
                if (!paciente.Sexo)
                {
                    if(request.Ginecobstetricos.FlujoVaginalId == null || request.Ginecobstetricos.TipoAnticonceptivoId == null)
                        throw new BadRequestException("Los campos de flujo vaginal y tipo de anticonceptivo son requeridos");
                    
                    var ginecobtetrico = new GinecoObstetrico()
                    {
                        Fum = request.Ginecobstetricos.Fum ?? "Sin registro",
                        Fpp = request.Ginecobstetricos.Fpp ?? "Sin registro",
                        Menarca = request.Ginecobstetricos.Menarca ?? "Sin registro",
                        Ritmo = request.Ginecobstetricos.Ritmo ?? "Sin registro",
                        Cirugias = request.Ginecobstetricos.Cirugias ?? "Sin registro",
                        EdadGestional = request.Ginecobstetricos.EdadGestional ?? 0,
                        Semanas = request.Ginecobstetricos.Semanas ?? 0,
                        Gestas = request.Ginecobstetricos.Gestas ?? 0,
                        Partos = request.Ginecobstetricos.Partos ?? 0,
                        Cesareas = request.Ginecobstetricos.Cesareas ?? 0,
                        Abortos = request.Ginecobstetricos.Abortos ?? 0,
                        FlujoVaginalId = request.Ginecobstetricos.FlujoVaginalId.HashIdInt(),
                        TipoAnticonceptivoId = request.Ginecobstetricos.TipoAnticonceptivoId.HashIdInt(),
                        ExpedienteId = expedient.ExpedienteId
                    };
                    
                    await _context.GinecoObstetricos.AddAsync(ginecobtetrico);
                    await _context.SaveChangesAsync();
                }
                
                //Si todo sale bien commitiar 
                transaction.Commit();
            }
            catch (Exception ex)
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
    
    public string CodigoExpediente(int PacienteId)
    {
        var prefijo = "";
        for (int i = 0; i < 4 - PacienteId.ToString().Length; i++)
        {
            prefijo += "0";
        }
        
        Console.WriteLine(prefijo);
        Console.WriteLine(FormatDate.DateLocal().Year);
        
        var codigo = $"{prefijo}{PacienteId.ToString()}-{FormatDate.DateLocal().Year}";
        
        return codigo;
    }
}