using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Fisioterapeuta
{
    public Fisioterapeuta()
    {
        Pacientes = new HashSet<Paciente>();
        Citas = new HashSet<Cita>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FisioterapeutaId { get; set; }

    public string Nombre { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string? CedulaProfesional { get; set; }
    public bool Status { get; set; }
    
    public DateTime FechaRegistro { get; set; } = DateTime.Now.Date;
    
    public byte[]? FotoPerfil { get; set; }

    // Foreign Key
    public int? EspecialidadId { get; set; }
    
    // Configuración de relación uno a uno
    public virtual Cat_Especialidades Especialidades { get; set; } = null!;
    
    // Configuración de relación uno a muchos
    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
}