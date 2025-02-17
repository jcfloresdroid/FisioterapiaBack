using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Pacientes.queries;

public record GetExpedient : IRequest<GetExpedientResponse>
{
    public string PacienteId { get; set; }
}

public class GetExpedientHandler : IRequestHandler<GetExpedient, GetExpedientResponse>
{
    private readonly FisioContext _context;

    public GetExpedientHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<GetExpedientResponse> Handle(GetExpedient request, CancellationToken cancellationToken)
    {
        //Buscamos si el usuario cuenta ya con un expediente
        var expedient = await _context.Expedientes
            .AsNoTracking()
            .Include(x => x.HeredoFamiliar)
            .Include(x => x.NoPatologico)
            .FirstOrDefaultAsync(x => x.PacienteId == request.PacienteId.HashIdInt());
        
        if(expedient == null)
            throw new NotFoundException("El paciente no existe o no ha terminado su registro");

        //Buscamos sus datos
        var gineco = await _context.GinecoObstetricos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExpedienteId == expedient.ExpedienteId);
        
        var diagnosticos = await _context.Diagnosticos
            .AsNoTracking()
            .OrderByDescending(x => x.FechaInicio)
            .ThenByDescending(x => x.DiagnosticoId)
            .Where(x => x.ExpedienteId == expedient.ExpedienteId)
            .ToListAsync();
        
        var response = new GetExpedientResponse()
        {
            ExpedienteId = expedient.ExpedienteId.HashId(),
            Nomenclatura = expedient.Nomenclatura,
            TipoInterrogatorio = expedient.TipoInterrogatorio,
            Responsable = expedient.Responsable,
            Diagnosticos = diagnosticos.Select(x => new DiagnosticGet()
            {
                Diagnostico = x.Descripcion,
                DiagnosticoId = x.DiagnosticoId.HashId(),
                Status = x.Estatus,
                FechaInicio = x.FechaInicio,
                FechaAlta = x.FechaAlta
            }).ToList(),
            HeredoFamiliar = new FamilyHistoryGet()
            {
                Padres = expedient.HeredoFamiliar.Padres,
                PadresVivos = expedient.HeredoFamiliar.PadresVivos,
                PadresCausaMuerte = expedient.HeredoFamiliar?.PadresCausaMuerte,
                Hermanos = expedient.HeredoFamiliar.Hermanos,
                HermanosVivos = expedient.HeredoFamiliar.HermanosVivos,
                HermanosCausaMuerte = expedient.HeredoFamiliar.HermanosCausaMuerte,
                Hijos = expedient.HeredoFamiliar.Hijos,
                HijosVivos = expedient.HeredoFamiliar.HijosVivos,
                HijosCausaMuerte = expedient.HeredoFamiliar.HijosCausaMuerte,
                Dm = expedient.HeredoFamiliar.Dm,
                Hta = expedient.HeredoFamiliar.Hta,
                Cancer = expedient.HeredoFamiliar.Cancer,
                Alcoholismo = expedient.HeredoFamiliar.Alcoholismo,
                Tabaquismo = expedient.HeredoFamiliar.Tabaquismo,
                Drogas = expedient.HeredoFamiliar.Drogas
            },
            Antecedente = new AntecedentsGet()
            {
                AntecedentesPatologicos = expedient.AntecedentesPatologicos,
                MedioLaboral = expedient.NoPatologico.MedioLaboral,
                MedioSociocultural = expedient.NoPatologico.MedioSociocultural,
                MedioFisicoambiental = expedient.NoPatologico.MedioFisicoambiental
            },
            Ginecobstetricos = gineco == null ? null : new GinecobstetricoGet()
            {
                Fum = gineco.Fum,
                Fpp = gineco.Fpp,
                Menarca = gineco.Menarca,
                Ritmo = gineco.Ritmo,
                Cirugias = gineco.Cirugias,
                EdadGestional = gineco.EdadGestional == 0 ? "No aplica" : gineco.EdadGestional.ToString(),
                Semanas = gineco.Semanas == 0 ? "No aplica" : gineco.Semanas.ToString(),
                Gestas = gineco.Gestas == 0 ? "No aplica" : gineco.Gestas.ToString(),
                Partos = gineco.Partos == 0 ? "No aplica" : gineco.Partos.ToString(),
                Cesareas = gineco.Cesareas == 0 ? "No aplica" : gineco.Cesareas.ToString(),
                Abortos = gineco.Abortos == 0 ? "No aplica" : gineco.Abortos.ToString(),
                FlujoVaginalId = gineco.FlujoVaginalId.Value.HashId(),
                TipoAnticonceptivoId = gineco.TipoAnticonceptivoId.Value.HashId()
            }
        };

        return response;
    }
}

public record GetExpedientResponse
{
    public string ExpedienteId { get; set; }
    
    public string Nomenclatura { get; set; }
    
    public bool TipoInterrogatorio { get; set; }
    
    public string Responsable { get; set; }
    
    public FamilyHistoryGet HeredoFamiliar { get; set; }
    
    public AntecedentsGet Antecedente { get; set; }
    
    public GinecobstetricoGet? Ginecobstetricos { get; set; }
    
    public List<DiagnosticGet> Diagnosticos { get; set; }
};

public record FamilyHistoryGet
{
    public int Padres { get; set; }

    public int PadresVivos { get; set; }

    public string PadresCausaMuerte { get; set; }

    public int Hermanos { get; set; }

    public int HermanosVivos { get; set; }

    public string HermanosCausaMuerte { get; set; }

    public int Hijos { get; set; }

    public int HijosVivos { get; set; }

    public string HijosCausaMuerte { get; set; }

    public string Dm { get; set; }

    public string Hta { get; set; }

    public string Cancer { get; set; }

    public string Alcoholismo { get; set; }

    public string Tabaquismo { get; set; }

    public string Drogas { get; set; }
}

public record AntecedentsGet
{
    public string AntecedentesPatologicos { get; set; }

    public string MedioLaboral { get; set; }

    public string MedioSociocultural { get; set; }

    public string MedioFisicoambiental { get; set; }
}

public record GinecobstetricoGet
{
    public string Fum { get; set; }

    public string Fpp { get; set; }

    public string EdadGestional { get; set; }

    public string Semanas { get; set; }

    public string Menarca { get; set; }

    public string Ritmo { get; set; }

    public string Gestas { get; set; }

    public string Partos { get; set; }

    public string Cesareas { get; set; }

    public string Abortos { get; set; }

    public string Cirugias { get; set; }

    public string FlujoVaginalId { get; set; }
    
    public string TipoAnticonceptivoId { get; set; }
}

public record DiagnosticGet
{
    public string Diagnostico { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaAlta { get; set; }
    public bool Status { get; set; }
    public string DiagnosticoId { get; set; } 
}