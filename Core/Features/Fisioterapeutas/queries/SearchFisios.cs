using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features;

public record SearchFisios() : IRequest<SearchFisiosResponse>
{
    public int Pagina { get; set; }
    public string Nombre { get; set; }
    public bool OnlyActive { get; set; }
};

public class SearchFisiosHandler : IRequestHandler<SearchFisios, SearchFisiosResponse>
{
    private readonly FisioContext _context;

    public SearchFisiosHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<SearchFisiosResponse> Handle(SearchFisios request, CancellationToken cancellationToken)
    {
        // Obtener el número total de fisios que cumplen con el criterio de búsqueda
        var pageFisios = await _context.Fisioterapeuta
            .AsNoTracking()
            .Where(x => x.Nombre.Contains(request.Nombre.Trim()) && (!request.OnlyActive || x.Status)) //Si solo quiero los activos o todos
            .ToListAsync();
        
        // Calcular el número de páginas
        int numPage = (int)Math.Ceiling((double)pageFisios.Count / 10);
        
        var listFisios = await _context.Fisioterapeuta
            .AsNoTracking()
            .Include(x => x.Especialidades)
            .Where(x => x.Nombre.Contains(request.Nombre.Trim()) && (!request.OnlyActive || x.Status))
            .Skip((request.Pagina - 1) * 10)
            .Take(10)
            .ToListAsync();
        
        // Ordenar los fisios según cada letra en la cadena de búsqueda
        listFisios = listFisios
            .OrderBy(p => 
            {
                //Convertimos el nombre a minusculas
                string nombre = p.Nombre.ToLower();
                //Esto es lo que estamos buscando
                string criterio = request.Nombre.ToLower();
                //Obtener los índices de cada letra en el nombre
                int[] indices = new int[criterio.Length];
                
                for (int i = 0; i < criterio.Length; i++)
                {
                    indices[i] = nombre.IndexOf(criterio[i]);
                    if (indices[i] == -1)
                    {
                        indices[i] = int.MaxValue;
                    }
                }

                // Convertir los índices a una cadena para usarla en la comparación
                return string.Join(",", indices);
            })
            .ThenBy(p => p.Nombre)
            .ToList();
        
        // Aplicar paginación
        var fisios = listFisios
            .Select(x => new SearchFisiosDto()
            {
                FisioterapeutaId = x.FisioterapeutaId.HashId(),
                Nombre = x.Nombre,
                CedulaProfesional = x.CedulaProfesional,
                Correo = x.Correo,
                Telefono = x.Telefono,
                Status = x.Status,
                Especialidad = x.Especialidades.Descripcion,
                Foto = x.FotoPerfil
            })
            .ToList();
        
        return await Task.FromResult(new SearchFisiosResponse()
        {
            NumPaginas = numPage,
            Total = pageFisios.Count,
            Fisioterapeutas = fisios
        });
    }
}

public record SearchFisiosResponse
{
    public int NumPaginas { get; set; }
    public int Total { get; set; }
    public List<SearchFisiosDto> Fisioterapeutas { get; set; }
}

public record SearchFisiosDto
{
    public string FisioterapeutaId { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string CedulaProfesional { get; set; }
    public byte[] Foto { get; set; }
    public bool Status { get; set; }
    public string Especialidad { get; set; }
}