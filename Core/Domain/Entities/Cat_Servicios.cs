using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Cat_Servicios
{
    public Cat_Servicios()
    {
        Revisions = new HashSet<Revision>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ServiciosId { get; set; }

    public string Descripcion { get; set; } = null!;
    
    public bool Status { get; set; }
    
    // Configuración de relación uno a muchos
    public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();
}