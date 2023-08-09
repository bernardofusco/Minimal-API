using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SuprimentosApi.Models;

[Table("MATERIAL")]
public class Material
{
	[Key]
	public int COD_EMPRESA { get; set; }
	[Key]
	public int COD_MAT { get; set; }
	public string? DESCRICAO { get; set; }
	public decimal? PRECO_UNIT { get; set; }
	[ForeignKey("Categoria")]
	public int COD_TIP_MAT { get; set; }
	[JsonIgnore]
	public Categoria Categoria { get; set; }
}