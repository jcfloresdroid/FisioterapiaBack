using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class ProgramaFisioterapeutico
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProgramaFisioterapeuticoId { get; set; }

    public string CortoPlazo { get; set; } = null!;
    public string MedianoPlazo { get; set; } = null!;
    public string LargoPlazo { get; set; } = null!;
    public string TratamientoFisioterapeutico { get; set; } = null!;
    public string Sugerencias { get; set; } = null!;
    public string Pronostico { get; set; } = null!;

    // Configuración de relación uno a uno
    public virtual Diagnostico Diagnostico { get; set; }
}
