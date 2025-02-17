using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class GinecoObstetrico
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GinecoObstetricoId { get; set; }

    public string Fum { get; set; } = null!;
    public string Fpp { get; set; } = null!;
    public string Menarca { get; set; } = null!;
    public string Ritmo { get; set; } = null!;
    public string Cirugias { get; set; } = null!;
    public int EdadGestional { get; set; }
    public int Semanas { get; set; }
    public int Gestas { get; set; }
    public int Partos { get; set; }
    public int Cesareas { get; set; }
    public int Abortos { get; set; }
    
    // Foreign keys
    public int? FlujoVaginalId { get; set; }
    public int? TipoAnticonceptivoId { get; set; }
    
    public int ExpedienteId { get; set; }

    // Configuración de relación uno a uno
    public virtual Cat_FlujoVaginal CatFlujoVaginal { get; set; }
    public virtual Cat_TipoAnticonceptivo CatTipoAnticonceptivo { get; set; }
    public virtual Expediente Expediente { get; set; }
}
