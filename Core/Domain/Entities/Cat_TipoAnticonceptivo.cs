using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Cat_TipoAnticonceptivo
{
    public Cat_TipoAnticonceptivo()
    {
        GinecoObstetricos = new HashSet<GinecoObstetrico>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TipoAnticonceptivoId { get; set; }

    public string Descripcion { get; set; } = null!;
    
    public bool Status { get; set; }

    // Configuración de relación uno a muchos
    public virtual ICollection<GinecoObstetrico> GinecoObstetricos { get; set; } = new List<GinecoObstetrico>();
}
