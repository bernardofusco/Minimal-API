using SuprimentosApi.Models;
using SuprimentosApi.Services;

namespace SuprimentosApi.ApiEndpoints;

public static class AutenticacaoEndpoints
{
	public static void MapAutenticacaoEndpoints(this WebApplication app)
	{
		app.MapPost("/login", (UserModel user, ITokenService token) =>
		{
			if (user == null) return Results.BadRequest("Login Inválido");
			if (user.UserName == "aplic_suprimentos" && user.Password == "Suprimento$2023")
			{
				var tokenString = token.GetToken(app.Configuration["Jwt:Key"],
													app.Configuration["Jwt:Issuer"],
													app.Configuration["Jwt:Audience"],
													user);
				return Results.Ok(new { token = tokenString });
			}
			else
			{
				return Results.BadRequest("Login Inválido");
			}
		}).Produces(StatusCodes.Status400BadRequest)
		.Produces(StatusCodes.Status200OK)
		.WithName("Login")
		.WithTags("Autenticacao");
	}
}
