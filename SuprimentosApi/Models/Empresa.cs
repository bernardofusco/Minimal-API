using System.ComponentModel.DataAnnotations.Schema;

namespace SuprimentosApi.Models;

[Table("EMPRESA")]

public class Empresa
{
	public int COD_EMPRESA { get; set; }
	public string NOME_EMPRESA { get; set; }
	public string FANTASIA { get; set; }
}