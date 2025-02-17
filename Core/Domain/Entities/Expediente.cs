using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Expediente
{
    public Expediente()
    {
        Diagnosticos = new HashSet<Diagnostico>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ExpedienteId { get; set; }

    public string Nomenclatura { get; set; } = null!;
    public bool TipoInterrogatorio { get; set; }
    public string Responsable { get; set; } = null!;
    public string AntecedentesPatologicos { get; set; } = null!;

    // Foreign Key
    public int PacienteId { get; set; }
    public int HeredoFamiliarId { get; set; }
    public int NoPatologicoId { get; set; }

    // Configuración de relación uno a uno
    public virtual Paciente paciente { get; set; }
    public virtual GinecoObstetrico GinecoObstetrico { get; set; }
    public virtual HeredoFamiliar HeredoFamiliar { get; set; }
    public virtual NoPatologico NoPatologico { get; set; }
    
    // Configuración de relación uno a muchos
    public virtual ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
}
