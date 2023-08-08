using FsCheck;
using Microsoft.EntityFrameworkCore;
using SuprimentosApi.Models;

namespace SuprimentosApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Material>? Materiais { get; set; }

}

