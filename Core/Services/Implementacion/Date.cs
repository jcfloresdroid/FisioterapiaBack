using Core.Domain.Enum;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Implementacion;

public class Date : IDate
{
    private readonly FisioContext _context;
    
    public Date(FisioContext context)
    {
        _context = context;
    }
    
    public async Task ModifyDate()
    {
        var citas = await _context.Citas
            .Where(x => x.Status == (int)EstadoCita.Pendiente)
            .ToListAsync();
        
        // Recorre todas las citas en busca de las que ya pasaron
        foreach (var cita in citas)
        {
            if (cita.Fecha.Date <= FormatDate.DateLocal().Date)
            {
                if (FormatHour.More10Minutes(cita.Hora) <= FormatDate.DateLocal().TimeOfDay)
                    cita.Status = (int)EstadoCita.Inasistencia;
                
                if(cita.Fecha.Date < FormatDate.DateLocal().Date)
                    cita.Status = (int)EstadoCita.Inasistencia;
            }
        }
        
        await _context.SaveChangesAsync();
    }
}