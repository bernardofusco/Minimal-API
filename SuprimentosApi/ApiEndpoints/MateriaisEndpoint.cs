using Microsoft.EntityFrameworkCore;
using SuprimentosApi.Context;
using SuprimentosApi.Models;

namespace SuprimentosApi.ApiEndpoints;

public static class MateriaisEndpoint
{
	public static void MapMateriaissEndpoints(this WebApplication app)
	{
		string endpointBase = "/materiais";
		string tags = "Materiais";

		app.MapGet(endpointBase, async (AppDbContext db) => await db.Materiais.ToListAsync()).WithTags(tags);

		app.MapGet(endpointBase + "{empresa:int}/{id:int}", async (int empresa, int id, AppDbContext db) =>
		{
			return await db.Materiais.FindAsync(empresa, id)
							is Material material
							? Results.Ok(material)
							: Results.NotFound();
		}).WithTags(tags);

		app.MapPut(endpointBase + "{empresa:int}/{id:int}", async (int empresa, int id, Material material, AppDbContext db) =>
		{
			if (material.COD_EMPRESA != empresa && material.COD_MAT != id)
				return Results.BadRequest($"Material da empresa: {empresa} com Id: {id} inválido!");

			var materialDB = await db.Materiais.FindAsync(empresa, id);

			if (materialDB is null)
				return Results.NotFound($"Material da empresa: {empresa} com Id: {id} não encontrado!");

			materialDB.DESCRICAO = material.DESCRICAO;
			materialDB.PRECO_UNIT = material.PRECO_UNIT;
			materialDB.COD_TIP_MAT = material.COD_TIP_MAT;

			await db.SaveChangesAsync();
			return Results.Ok(materialDB);
		}).WithTags(tags);

		app.MapPost(endpointBase + "{empresa:int}", async (int empresa, Material material, AppDbContext db) =>
		{
			var empresaDB = await db.Empresas.FindAsync(empresa);
			if (empresaDB is null || material.COD_EMPRESA != empresa)
				return Results.BadRequest($"Empresa: {empresa} inválida!");

			if (material is null)
				return Results.BadRequest("Material Vazio!");

			db.Materiais.Add(material);
			await db.SaveChangesAsync();
			return Results.Created($"{endpointBase}/{material.COD_MAT}", material);

		}).WithTags(tags);

		app.MapDelete(endpointBase + "{empresa:int}/{id:int}", async (int empresa, int id, AppDbContext db) =>
		{
			var materialDB = await db.Materiais.FindAsync(empresa, id);
			if (materialDB is null)
				return Results.NotFound($"Material da empresa: {empresa} com Id: {id} não encontrado!");

			db.Materiais.Remove(materialDB);
			await db.SaveChangesAsync();
			return Results.NoContent();

		}).WithTags(tags);
	}
}
