using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class ExploracionFisica
{
    public ExploracionFisica()
    {
        Revisions = new HashSet<Revision>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ExploracionFisicaId { get; set; }

    public int Fr { get; set; }
    public int Fc { get; set; }
    public float Temperatura { get; set; }
    public float Peso { get; set; }
    public float Estatura { get; set; }
    public float Imc { get; set; }
    public float IndiceCinturaCadera { get; set; }
    public float SaturacionOxigeno { get; set; }
    public string PresionArterial { get; set; } = null!;

    // Configuración de relación uno a muchos
    public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();
}
