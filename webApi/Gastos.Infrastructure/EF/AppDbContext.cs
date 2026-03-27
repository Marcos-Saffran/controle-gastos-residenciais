using Gastos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gastos.Infrastructure.EF;

public class AppDbContext : DbContext
{
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}