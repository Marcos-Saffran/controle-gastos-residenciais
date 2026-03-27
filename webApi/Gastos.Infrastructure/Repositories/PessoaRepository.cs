using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Gastos.Infrastructure.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _context;

    public PessoaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pessoa> AddAsync(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        return pessoa;
    }

    public async Task<Pessoa?> ObterPorIdAsync(int id)
    {
        return await _context.Pessoas.FindAsync(id);
    }

    public async Task<List<Pessoa>> ListarAsync()
    {
        return await _context.Pessoas.ToListAsync();
    }

    public async Task DeleteAsync(Pessoa pessoa)
    {
        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}