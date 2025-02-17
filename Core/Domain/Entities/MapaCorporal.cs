using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Core.Domain.Entities;

public class MapaCorporal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MapaCorporalId { get; set; }

    [JsonProperty("valor")]
    public List<int> Valor { get; set; }
    
    [JsonProperty("rangodolor")]
    public List<int> RangoDolor { get; set; }
    public string Nota { get; set; } = null!;

    // Configuración de relación uno a uno
    public virtual Diagnostico Diagnostico { get; set; }
}
