using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Graficos.queries;

public record SexoPaciente : IRequest<SexoPacienteResponse>
{
    public int Meses { get; set; }
}

public class SexoPacienteHandler : IRequestHandler<SexoPaciente, SexoPacienteResponse>
{
    private readonly FisioContext _context;
    
    public SexoPacienteHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<SexoPacienteResponse> Handle(SexoPaciente request, CancellationToken cancellationToken)
    {
        var startDate = FormatDate.DateLocal().AddMonths(-request.Meses);
        
        var sexo = await _context.Pacientes
            .Where(x => x.Status && request.Meses == 0 ? true : x.FechaRegistro >= startDate)
            .ToListAsync();
        
        var masculino = sexo.Count(x => x.Sexo);
        var femenino = sexo.Count(x => !x.Sexo);
        
        var response = new SexoPacienteResponse()
        {
            Masculino = Math.Round(((double)masculino / (double)sexo.Count) * 100, 2),
            Feminino = Math.Round(((double)femenino / (double)sexo.Count) * 100, 2)
        };

        return response;
    }
}

public record SexoPacienteResponse
{
    public double Masculino { get; set; }
    public double Feminino { get; set; }
}