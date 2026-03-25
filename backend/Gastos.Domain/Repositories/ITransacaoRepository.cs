using Gastos.Domain.Entities;

namespace Gastos.Domain.Repositories
{
    public interface ITransacaoRepository
    {
        Task<Transacao> AddAsync(Transacao transacao);
        Task<Transacao?> ObterPorIdAsync(int id);
        Task<List<Transacao>> ListarAsync();
        Task DeleteByPessoaIdAsync(int pessoaId);
        Task SaveChangesAsync();
    }
}
