using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Pacientes.Command;

public record ModificacionExpedienteHeredo
{
    public int? Padres { get; set; }
    public int? PadresVivos { get; set; }
    public string? PadresCausaMuerte { get; set; }
    public int? Hermanos { get; set; }
    public int? HermanosVivos { get; set; }
    public string? HermanosCausaMuerte { get; set; }
    public int? Hijos { get; set; }
    public int? HijosVivos { get; set; }
    public string? HijosCausaMuerte { get; set; }
    public string? Dm { get; set; }
    public string? Hta { get; set; }
    public string? Cancer { get; set; }
    public string? Alcoholismo { get; set; }
    public string? Tabaquismo { get; set; }
    public string? Drogas { get; set; }
}

public record ModificarExpedienteAntecedente
{
    public string? AntecedentesPatologicos { get; set; }
    public string? MedioLaboral { get; set; }
    public string? MedioSociocultural { get; set; }
    public string? MedioFisicoambiental { get; set; }
}

public record ModificarExpedienteGineco
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

public record ModificacionExpediente : IRequest
{
    public string ExpedienteId { get; set; }
    
    public bool? TipoInterrogatorio { get; set; }
    
    public string? Responsable { get; set; }

    public ModificacionExpedienteHeredo? Heredo { get; set; }
    public ModificarExpedienteAntecedente? Antecedente { get; set; }
    public ModificarExpedienteGineco? Gineco { get; set; }
}

public class ModificarExpedienteHandler : IRequestHandler<ModificacionExpediente>
{
    private readonly FisioContext _context;

    public ModificarExpedienteHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task Handle(ModificacionExpediente request, CancellationToken cancellationToken)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var expediente = await _context.Expedientes
                    .Include(x => x.HeredoFamiliar)
                    .Include(x => x.NoPatologico)
                    .FirstOrDefaultAsync(x => x.ExpedienteId == request.ExpedienteId.HashIdInt());
                
                if (expediente == null)
                    throw new NotFoundException("El expediente con ese Id no existe");

                expediente.TipoInterrogatorio = request.TipoInterrogatorio ?? expediente.TipoInterrogatorio;
                
                if (!expediente.TipoInterrogatorio)
                    expediente.Responsable = request.Responsable ?? expediente.Responsable;

                if (request.Antecedente != null)
                {
                    expediente.AntecedentesPatologicos = request.Antecedente.AntecedentesPatologicos ?? expediente.AntecedentesPatologicos;
                    expediente.NoPatologico.MedioLaboral = request.Antecedente.MedioLaboral ?? expediente.NoPatologico.MedioLaboral;
                    expediente.NoPatologico.MedioSociocultural = request.Antecedente.MedioSociocultural ?? expediente.NoPatologico.MedioSociocultural;
                    expediente.NoPatologico.MedioFisicoambiental = request.Antecedente.MedioFisicoambiental ?? expediente.NoPatologico.MedioFisicoambiental;
                }

                if (request.Heredo != null)
                {
                    expediente.HeredoFamiliar.Padres = request.Heredo.Padres ?? expediente.HeredoFamiliar.Padres;
                    expediente.HeredoFamiliar.PadresVivos = request.Heredo.PadresVivos ?? expediente.HeredoFamiliar.PadresVivos;
                    expediente.HeredoFamiliar.PadresCausaMuerte = request.Heredo.PadresCausaMuerte ?? expediente.HeredoFamiliar.PadresCausaMuerte;
                    expediente.HeredoFamiliar.Hermanos = request.Heredo.Hermanos ?? expediente.HeredoFamiliar.Hermanos;
                    expediente.HeredoFamiliar.HermanosVivos = request.Heredo.HermanosVivos ?? expediente.HeredoFamiliar.HermanosVivos;
                    expediente.HeredoFamiliar.HermanosCausaMuerte = request.Heredo.HermanosCausaMuerte ?? expediente.HeredoFamiliar.HermanosCausaMuerte;
                    expediente.HeredoFamiliar.Hijos = request.Heredo.Hijos ?? expediente.HeredoFamiliar.Hijos;
                    expediente.HeredoFamiliar.HijosVivos = request.Heredo.HijosVivos ?? expediente.HeredoFamiliar.HijosVivos;
                    expediente.HeredoFamiliar.HijosCausaMuerte = request.Heredo.HijosCausaMuerte ?? expediente.HeredoFamiliar.HijosCausaMuerte;
                    expediente.HeredoFamiliar.Dm = request.Heredo.Dm ?? expediente.HeredoFamiliar.Dm;
                    expediente.HeredoFamiliar.Hta = request.Heredo.Hta ?? expediente.HeredoFamiliar.Hta;
                    expediente.HeredoFamiliar.Cancer = request.Heredo.Cancer ?? expediente.HeredoFamiliar.Cancer;
                    expediente.HeredoFamiliar.Alcoholismo = request.Heredo.Alcoholismo ?? expediente.HeredoFamiliar.Alcoholismo;
                    expediente.HeredoFamiliar.Tabaquismo = request.Heredo.Tabaquismo ?? expediente.HeredoFamiliar.Tabaquismo;
                    expediente.HeredoFamiliar.Drogas = request.Heredo.Drogas ?? expediente.HeredoFamiliar.Drogas;
                }

                if (request.Gineco != null)
                {
                    var expedienteGineco = await _context.GinecoObstetricos
                        .FirstOrDefaultAsync(x => x.ExpedienteId == expediente.ExpedienteId);

                    expedienteGineco.Fum = request.Gineco.Fum ?? expediente.GinecoObstetrico.Fum;
                    expedienteGineco.Fpp = request.Gineco.Fpp ?? expediente.GinecoObstetrico.Fpp;
                    expedienteGineco.Menarca = request.Gineco.Menarca ?? expediente.GinecoObstetrico.Menarca;
                    expedienteGineco.Ritmo = request.Gineco.Ritmo ?? expediente.GinecoObstetrico.Ritmo;
                    expedienteGineco.Cirugias = request.Gineco.Cirugias ?? expediente.GinecoObstetrico.Cirugias;
                    expedienteGineco.EdadGestional = request.Gineco.EdadGestional ?? expediente.GinecoObstetrico.EdadGestional;
                    expedienteGineco.Semanas = request.Gineco.Semanas ?? expediente.GinecoObstetrico.Semanas;
                    expedienteGineco.Gestas = request.Gineco.Gestas ?? expediente.GinecoObstetrico.Gestas;
                    expedienteGineco.Partos = request.Gineco.Partos ?? expediente.GinecoObstetrico.Partos;
                    expedienteGineco.Cesareas = request.Gineco.Cesareas ?? expediente.GinecoObstetrico.Cesareas;
                    expedienteGineco.Abortos = request.Gineco.Abortos ?? expediente.GinecoObstetrico.Abortos;
                    expedienteGineco.FlujoVaginalId = request.Gineco.FlujoVaginalId.HashIdInt();
                    expedienteGineco.TipoAnticonceptivoId = request.Gineco.TipoAnticonceptivoId.HashIdInt();
                    
                    _context.GinecoObstetricos.Update(expedienteGineco);
                }
                
                _context.Expedientes.Update(expediente);
                await _context.SaveChangesAsync();

                //Si todo sale bien commitiar 
                transaction.Commit();
            }
            catch (Exception e)
            {
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