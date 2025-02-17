using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Cat_FlujoVaginal
{
    public Cat_FlujoVaginal()
    {
        GinecoObstetricos = new HashSet<GinecoObstetrico>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FlujoVaginalId { get; set; }

    public string Descripcion { get; set; } = null!;
    
    public bool Status { get; set; }

    // Configuración de relación uno a muchos
    public virtual ICollection<GinecoObstetrico> GinecoObstetricos { get; set; } = new List<GinecoObstetrico>();
}
