using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Revision
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionId { get; set; }

    public string Notas { get; set; } = null!;
    public string FolioPago { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }

    // Foreign keys
    public int ExploracionFisicaId { get; set; }
    public int DiagnosticoId { get; set; }
    public int? ServicioId { get; set; }

    // Configuración de relación uno a uno
    public virtual Diagnostico Diagnostico { get; set; }
    public virtual ExploracionFisica ExploracionFisica { get; set; }
    public virtual Cat_Servicios Servicio { get; set; }

}
