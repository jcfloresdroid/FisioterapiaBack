using Core.Domain.Enum;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Graficos.queries;

public record GraphicsPatient : IRequest<GraphicsPatientResponse>
{
    
}

public class GraphicsPatientHandler : IRequestHandler<GraphicsPatient, GraphicsPatientResponse>
{
    private readonly FisioContext _context;
    
    public GraphicsPatientHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<GraphicsPatientResponse> Handle(GraphicsPatient request, CancellationToken cancellationToken)
    {
        var pacientes = await _context.Pacientes
            .AsNoTracking()
            .Where(x => x.FechaRegistro.Date >= FormatDate.StartOfWeek().Date && x.FechaRegistro.Date <= FormatDate.EndOfWeek().Date)
            .ToListAsync();
        
        //Obtenemos la cantidad de pacientes que hubo respecto a la semana pasada
        var startOfLastWeek = FormatDate.StartOfWeek().AddDays(-7).Date;
        var endOfLastWeek = FormatDate.EndOfWeek().AddDays(-7).Date;

        var pacientesLast = await _context.Pacientes
            .AsNoTracking()
            .Where(x => x.FechaRegistro.Date >= startOfLastWeek && x.FechaRegistro.Date <= endOfLastWeek)
            .ToListAsync();
        
        return await Task.FromResult(new GraphicsPatientResponse()
        {
            semana = new Semana
            {
                Lunes = pacientes.Count(x => x.FechaRegistro.DayOfWeek == DayOfWeek.Monday),
                Martes = pacientes.Count(x => x.FechaRegistro.DayOfWeek == DayOfWeek.Tuesday),
                Miercoles = pacientes.Count(x => x.FechaRegistro.DayOfWeek == DayOfWeek.Wednesday),
                Jueves = pacientes.Count(x => x.FechaRegistro.DayOfWeek == DayOfWeek.Thursday),
                Viernes = pacientes.Count(x => x.FechaRegistro.DayOfWeek == DayOfWeek.Friday),
                Sabado = pacientes.Count(x => x.FechaRegistro.DayOfWeek == DayOfWeek.Saturday),
                Domingo = pacientes.Count(x => x.FechaRegistro.DayOfWeek == DayOfWeek.Sunday)
            },
            Porcentaje = pacientesLast.Count != 0 ? Math.Round((((double)pacientes.Count - (double)pacientesLast.Count) / (double)pacientesLast.Count) * (double)100, 2) : 100
        });
    }
}

public record GraphicsPatientResponse
{
    public Semana semana { get; set; }
    
    public int Total => semana.Lunes + semana.Martes + semana.Miercoles + semana.Jueves + semana.Viernes + semana.Sabado + semana.Domingo;
    
    public double Porcentaje { get; set; }
}

public record Semana
{
    public int Lunes { get; set; }
    public int Martes { get; set; }
    public int Miercoles { get; set; }
    public int Jueves { get; set; }
    public int Viernes { get; set; }
    public int Sabado { get; set; }
    public int Domingo { get; set; }
}