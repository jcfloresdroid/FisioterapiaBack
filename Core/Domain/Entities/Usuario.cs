using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UsuarioId { get; set; }

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Clave { get; set; } = null!;
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    
    public DateTime FechaRegistro { get; set; } = DateTime.Now.Date;
    
    public byte[]? FotoPerfil { get; set; }
    
    // Foreign Key
    public int? EspecialidadId { get; set; }
    
    // Configuración de relación uno a uno
    public virtual Cat_Especialidades Especialidades { get; set; } = null!;
}