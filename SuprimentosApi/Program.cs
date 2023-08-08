using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuprimentosApi.Context;
using SuprimentosApi.Models;
using SuprimentosApi.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(
	option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string oracleConnection = builder.Configuration.GetConnectionString("OracleConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseOracle(oracleConnection));

builder.Services.AddSingleton<ITokenService>(new TokenService());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	};
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapPost("/login", [AllowAnonymous] (UserModel user, ITokenService token) =>
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

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();