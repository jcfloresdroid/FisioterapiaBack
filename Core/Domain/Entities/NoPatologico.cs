using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class NoPatologico
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NoPatologicoId { get; set; }

    public string MedioLaboral { get; set; } = null!;
    public string MedioSociocultural { get; set; } = null!;
    public string MedioFisicoambiental { get; set; } = null!;
    
    // Configuración de relación uno a uno
    public virtual Expediente Expediente { get; set; }
}
