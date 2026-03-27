using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Gastos.Infrastructure.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Transacao> AddAsync(Transacao transacao)
    {
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        return transacao;
    }

    public async Task<Transacao?> ObterPorIdAsync(int id)
    {
        return await _context.Transacoes.FindAsync(id);
    }

    public async Task<List<Transacao>> ListarAsync()
    {
        return await _context.Transacoes
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task DeleteByPessoaIdAsync(int pessoaId)
    {
        var transacoes = await _context.Transacoes
            .Where(t => t.PessoaId == pessoaId)
            .ToListAsync();

        if (transacoes.Count == 0)
        {
            return;
        }

        _context.Transacoes.RemoveRange(transacoes);
        await _context.SaveChangesAsync();
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}