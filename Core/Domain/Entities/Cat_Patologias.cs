using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Cat_Patologias
{
    public Cat_Patologias()
    {
        Diagnosticos = new HashSet<Diagnostico>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PatologiasId { get; set; }

    public string Descripcion { get; set; } = null!;
    
    public bool Status { get; set; }

    // Configuración de relación uno a muchos
    public virtual ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
}