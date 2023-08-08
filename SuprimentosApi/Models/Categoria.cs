using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SuprimentosApi.Models;

[Table("TIPO_MAT")]
public class Categoria
{
    public Categoria()
    {
        Materiais = new Collection<Material>();
    }
    [Key]
    public int COD_TIP_MAT { get; set; }
    public string? DESC_TIP_MAT { get; set; }
    [JsonIgnore]
    public ICollection<Material> Materiais { get; set; }

}