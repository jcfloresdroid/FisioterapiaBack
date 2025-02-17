using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Paciente
{
    public Paciente()
    {
        Citas = new HashSet<Cita>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PacienteId { get; set; }

    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Institucion { get; set; } = null!;
    public string Domicilio { get; set; } = null!;
    public string Ocupacion { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string? Notas { get; set; }
    public bool Sexo { get; set; }
    public bool TipoPaciente { get; set; } //True = Interno, False = Externo
    public bool Status { get; set; } //True = Activo, False = Inactivo
    public DateTime Edad { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int CodigoPostal { get; set; }
    
    public byte[]? Foto { get; set; }
    
    // Foreign keys
    public int? EstadoCivilId { get; set; }
    public int? FisioterapeutaId { get; set; }

    // Configuración de relación uno a uno
    public virtual Expediente Expediente { get; set; }
    public virtual Cat_EstadoCivil CatEstadoCivil { get; set; }
    public virtual Fisioterapeuta Fisioterapeuta { get; set; }
    
    // Configuración de relación uno a muchos
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
