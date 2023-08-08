using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuprimentosApi.Context;
using SuprimentosApi.Models;

namespace SuprimentosApi.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
	private readonly AppDbContext _context;

	public CategoriasController(AppDbContext context) => _context = context;

	[HttpGet("Materiais")]
	public ActionResult<IEnumerable<Categoria>> GetCategoriaMateriais()
	{
		try
		{
			var categorias = _context.Categorias.Include(m => m.Materiais).AsNoTracking().ToList();

			if (categorias is null) return NotFound("Categorias não encontradas...");

			return categorias;
		}
		catch (Exception)
		{
			return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
		}
	}

	[HttpGet]
	public ActionResult<IEnumerable<Categoria>> Get()
	{
		try
		{
			var categorias = _context.Categorias.AsNoTracking().ToList();

			if (categorias is null) return NotFound("Categorias não encontradas...");

			return categorias;
		}
		catch (Exception)
		{
			return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
		}
	}

	[HttpGet("{cod_categoria:int}", Name = "ObterCategoria")]
	public ActionResult<Categoria> Get(int cod_categoria)
	{
		try
		{
			var categoria = _context.Categorias.FirstOrDefault(c => c.COD_TIP_MAT == cod_categoria);

			if (categoria is null) return NotFound("Categoria não encontrada...");

			return Ok(categoria);
		}
		catch (Exception)
		{
			return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
		}
	}

	[HttpPost]
	public ActionResult Post(Categoria categoria)
	{
		try
		{
			if (categoria is null) return BadRequest();

			_context.Categorias.Add(categoria);
			_context.SaveChanges();

			return new CreatedAtRouteResult("ObterCategoria",
				new { cod_categoria = categoria.COD_TIP_MAT }, categoria);
		}
		catch (Exception)
		{
			return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
		}
	}

	[HttpPut("{cod_categoria:int}")]
	public ActionResult Put(int cod_categoria, Categoria categoria)
	{
		try
		{
			if (categoria is null || cod_categoria != categoria.COD_TIP_MAT) return BadRequest();

			_context.Entry(categoria).State = EntityState.Modified;
			_context.SaveChanges();

			return Ok(categoria);
		}
		catch (Exception)
		{
			return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
		}
	}

	[HttpDelete("{cod_categoria:int}")]
	public ActionResult Delete(int cod_categoria)
	{
		try
		{
			var categoria = _context.Categorias.FirstOrDefault(c => c.COD_TIP_MAT == cod_categoria);

			if (categoria is null) return NotFound();

			_context.Categorias.Remove(categoria);
			_context.SaveChanges();

			return Ok(categoria);
		}
		catch (Exception)
		{
			return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
		}
	}
}
