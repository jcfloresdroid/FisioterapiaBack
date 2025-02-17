using System.Globalization;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Graficos.queries;

public record EdadesPaciente : IRequest<EdadesPacienteResponse>
{
    public int Meses { get; set; }
}

public class EdadesPacienteHandler : IRequestHandler<EdadesPaciente, EdadesPacienteResponse>
{
    private readonly FisioContext _context;
    
    public EdadesPacienteHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<EdadesPacienteResponse> Handle(EdadesPaciente request, CancellationToken cancellationToken)
    {
        DateTime StartMonth = new DateTime(FormatDate.DateLocal().Year, FormatDate.DateLocal().Month, 1);
        DateTime EndMonth = StartMonth.AddMonths(1).AddDays(-1);
        int menor = 100;
        int mayor = 0;
        int promedio = 0;

        List<EdadesPacientMesResponse> mes = new List<EdadesPacientMesResponse>();

        for (int i = 0; i < request.Meses; i++)
        {
            var paciente = await _context.Pacientes
                .Where(x => x.FechaRegistro >= StartMonth && x.FechaRegistro <= EndMonth && x.Status)
                .ToListAsync();

            int promedioMes = 0;
            
            foreach (var item in paciente)
            {
                if(menor > FormatDate.DateToYear(item.Edad))
                    menor = FormatDate.DateToYear(item.Edad);
                
                if(mayor < FormatDate.DateToYear(item.Edad))
                    mayor = FormatDate.DateToYear(item.Edad);
                
                promedioMes += FormatDate.DateToYear(item.Edad);
            }
            
            promedio += (paciente.Count == 0 ? 0 : promedioMes / paciente.Count);
            
            mes.Add(new EdadesPacientMesResponse
            {
                Mes = meses[StartMonth.Month],
                Promedio = paciente.Count == 0 ? 0 : promedioMes / paciente.Count
            });
            
            StartMonth = StartMonth.AddMonths(-1);
            EndMonth = StartMonth.AddMonths(1).AddDays(-1);
        }
        
        return new EdadesPacienteResponse
        {
            Menor = menor,
            Mayor = mayor,
            Promedio = promedio / request.Meses,
            Mes = mes
        };
    }
    
    Dictionary<int, string> meses = new Dictionary<int, string>()
    {
        {1, "Ene"},
        {2, "Feb"},
        {3, "Mar"},
        {4, "Abr"},
        {5, "May"},
        {6, "Jun"},
        {7, "Jul"},
        {8, "Ago"},
        {9, "Sept"},
        {10, "Oct"},
        {11, "Nov"},
        {12, "Dic"}
    };
}

public record EdadesPacienteResponse
{
    public int Menor { get; set; }
    public int Mayor { get; set; }
    public int Promedio { get; set; }
    public List<EdadesPacientMesResponse> Mes { get; set; }
}

public record EdadesPacientMesResponse
{
    public string Mes { get; set; }
    public int Promedio { get; set; }
}