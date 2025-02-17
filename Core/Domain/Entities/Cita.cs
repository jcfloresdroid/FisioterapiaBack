using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Cita
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CitasId { get; set; }

    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }
    public string Motivo { get; set; } = null!;
    public int Status { get; set; }
    
    // Foreign Key
    public int PacienteId { get; set; }
    public int? FisioterapeutaId { get; set; }

    // Configuración de relación uno a uno
    public virtual Paciente Paciente { get; set; }
    public virtual Fisioterapeuta Fisio { get; set; }
}
