using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class HeredoFamiliar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HeredoFamiliarId { get; set; }

    public int Padres { get; set; }
    public int PadresVivos { get; set; }
    public string? PadresCausaMuerte { get; set; }
    public int Hermanos { get; set; }
    public int HermanosVivos { get; set; }
    public string? HermanosCausaMuerte { get; set; }
    public int Hijos { get; set; }
    public int HijosVivos { get; set; }
    public string? HijosCausaMuerte { get; set; }
    public string Dm { get; set; } = null!;
    public string Hta { get; set; } = null!;
    public string Cancer { get; set; } = null!;
    public string Alcoholismo { get; set; } = null!;
    public string Tabaquismo { get; set; } = null!;
    public string Drogas { get; set; } = null!;
    
    // Configuración de relación uno a uno
    public virtual Expediente Expediente { get; set; }
    
}
