using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Diagnostico
{
    public Diagnostico()
    {
        Revisions = new HashSet<Revision>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DiagnosticoId { get; set; }

    public string Descripcion { get; set; } = null!;
    public string Refiere { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string DiagnosticoPrevio { get; set; } = null!;
    public string TerapeuticaEmpleada { get; set; } = null!;
    public string DiagnosticoFuncional { get; set; } = null!;
    public string PadecimientoActual { get; set; } = null!;
    public string Inspeccion { get; set; } = null!;
    public string ExploracionFisicaCuadro { get; set; } = null!;
    public string EstudiosComplementarios { get; set; } = null!;
    public string DiagnosticoNosologico { get; set; } = null!;
    public string? DiagnosticoInicial { get; set; } //Se llena al finalizar el tratamiento
    public string? DiagnosticoFinal { get; set; } //Se llena al finalizar el tratamiento
    public string? FrecuenciaTratamiento { get; set; } //Se llena al finalizar el tratamiento
    public bool Estatus { get; set; } // 1 Activo 0 Inactivo
    public DateTime? FechaAlta { get; set; } //Se llena al finalizar el tratamiento
    public DateTime FechaInicio { get; set; }

    // Foreign Key
    public int? MotivoAltaId { get; set; } //Se llena al finalizar el tratamiento
    
    public int? PatologiasId { get; set; }
    public int ProgramaFisioterapeuticoId { get; set; }
    public int MapaCorporalId { get; set; }
    public int ExpedienteId { get; set; }

    // Configuración de relación uno a uno
    public virtual ProgramaFisioterapeutico ProgramaFisioterapeutico { get; set; }
    public virtual MapaCorporal MapaCorporal { get; set; }
    public virtual Expediente Expediente { get; set; }
    public virtual Cat_MotivoAlta MotivoAlta { get; set; }
    public virtual Cat_Patologias Patologias { get; set; }

    // Configuración de relación uno a muchos
    public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();
}
