using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuprimentosApi.Context;
using SuprimentosApi.Models;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace SuprimentosApi.ApiEndpoints;

public static class CategoriasEndpoints
{
	public static void MapCategoriasEndpoints(this WebApplication app)
	{
		string endpointBase = "/categorias";
		string tags = "Categorias";

		app.MapGet(endpointBase, async (AppDbContext db) => await db.Categorias.ToListAsync()).WithTags(tags);

		app.MapGet(endpointBase, async (int id, AppDbContext db) =>
		{
			return await db.Categorias.FindAsync(id)
							is Categoria categoria
							? Results.Ok(categoria)
							: Results.NotFound($"Categoria com Id: {id} inválido!");
		}).WithTags(tags);

		app.MapPut(endpointBase + "/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
		{
			if (categoria.COD_TIP_MAT != id)
				return Results.BadRequest($"Categoria com Id: {id} inválida!");

			var categoriaDB = await db.Categorias.FindAsync(id);

			if (categoriaDB is null)
				return Results.NotFound($"Categoria com Id: {id} não encontrada!");

			categoriaDB.DESC_TIP_MAT = categoria.DESC_TIP_MAT;
			await db.SaveChangesAsync();
			return Results.Ok(categoriaDB);

		}).WithTags(tags); ;

		app.MapPost(endpointBase, async (Categoria categoria, AppDbContext db) =>
		{
			if (categoria is null)
				return Results.BadRequest("Categoria Vazia!");

			db.Categorias.Add(categoria);
			await db.SaveChangesAsync();
			return Results.Created($"{endpointBase}/{categoria.COD_TIP_MAT}", categoria);

		}).WithTags(tags);

		app.MapDelete(endpointBase + "/{id:int}", async (int id, AppDbContext db) =>
		{
			var categoria = await db.Categorias.FindAsync(id);

			if (categoria is null)
				return Results.NotFound($"Categoria com Id: {id} não encontrada!");

			db.Categorias.Remove(categoria);
			await db.SaveChangesAsync();
			return Results.NoContent();

		}).WithTags(tags);
	}
}