using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Cat_EstadoCivil
{
    public Cat_EstadoCivil()
    {
        Pacientes = new HashSet<Paciente>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EstadoCivilId { get; set; }

    public string Descripcion { get; set; } = null!;
    
    public bool Status { get; set; }

    // Configuración de relación uno a muchos
    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
