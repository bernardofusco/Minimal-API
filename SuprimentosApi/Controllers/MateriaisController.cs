using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuprimentosApi.Context;
using SuprimentosApi.Models;

namespace SuprimentosApi.Controllers;

[Route("[controller]")]
[ApiController]
public class MateriaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public MateriaisController(AppDbContext context) => _context = context;

    [HttpGet]
    public ActionResult<IEnumerable<Material>> Get()
    {
        try
        {
            var materiais = _context.Materiais.AsNoTracking().ToList();

            if (materiais is null) return NotFound("Materiais não encontrados...");

            return materiais;
            throw new Exception();
        }
        catch (Exception)
        {
            return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
        }
    }

    [HttpGet("{empresa:int}/{cod_material:int}", Name = "ObterMaterial")]
    public ActionResult<Material> Get(int empresa, int cod_material)
    {
        try
        {
            var material = _context.Materiais.FirstOrDefault(m => m.COD_EMPRESA == empresa && m.COD_MAT == cod_material);

            if (material is null) return NotFound("Material não encontrado...");

            return Ok(material);
        }
        catch (Exception)
        {
            return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
        }
    }

    [HttpPost]
    public ActionResult Post(Material material)
    {
        try
        {
            if (material is null) return BadRequest();

            _context.Materiais.Add(material);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterMaterial",
                new { empresa = material.COD_EMPRESA, cod_material = material.COD_MAT }
                , material
                );
        }
        catch (Exception)
        {
            return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
        }
    }

    [HttpPut("{empresa:int}/{cod_material:int}")]
    public ActionResult Put(int empresa, int cod_material, Material material)
    {
        try
        {
            if (material is null || empresa != material.COD_EMPRESA || cod_material != material.COD_MAT)
                return BadRequest();

            _context.Entry(material).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(material);
        }
        catch (Exception)
        {
            return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
        }
    }

    [HttpDelete("{empresa:int}/{cod_material:int}")]
    public ActionResult Delete(int empresa, int cod_material)
    {
        try
        {
            var material = _context.Materiais.FirstOrDefault(m => m.COD_EMPRESA == empresa && m.COD_MAT == cod_material);
            if (material is null) return NotFound("Material não encontrado!");

            _context.Materiais.Remove(material);
            _context.SaveChanges();

            return Ok("Material: " + material.DESCRICAO + " da Empresa: " + material.COD_EMPRESA + " deletado com sucesso!");
        }
        catch (Exception)
        {
            return StatusCode(500, "Problema ao executar solicitação. Favor procurar a Equipe de Desenvolvimento.");
        }
    }
}
