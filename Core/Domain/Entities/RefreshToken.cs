using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class RefreshToken {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RefreshTokenId { get; set; }
    
    public int UsuarioId { get; set; }
    
    public string Token { get; set; } = null!;
    
    public DateTime Expiracion { get; set; }
}