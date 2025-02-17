using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Cat_Especialidades
{
    public Cat_Especialidades()
    {
        Fisioterapeutas = new HashSet<Fisioterapeuta>();
        Usuarios = new HashSet<Usuario>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EspecialidadesId { get; set; }

    public string Descripcion { get; set; } = null!;
    
    public bool Status { get; set; }
    
    // Configuración de relación uno a muchos
    public virtual ICollection<Fisioterapeuta> Fisioterapeutas { get; set; } = new List<Fisioterapeuta>();
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}