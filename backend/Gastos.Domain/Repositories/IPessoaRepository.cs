using Gastos.Domain.Entities;

namespace Gastos.Domain.Repositories
{
    public interface IPessoaRepository
    {
        Task<Pessoa> AddAsync(Pessoa pessoa);

        Task<Pessoa?> ObterPorIdAsync(int id);
        Task<List<Pessoa>> ListarAsync();

        Task SaveChangesAsync();
    }
}
