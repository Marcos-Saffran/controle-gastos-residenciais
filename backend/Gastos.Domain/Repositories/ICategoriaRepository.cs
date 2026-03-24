using Gastos.Domain.Entities;

namespace Gastos.Domain.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AddAsync(Categoria categoria);
        Task<Categoria?> ObterPorIdAsync(int id);
        Task<List<Categoria>> ListarAsync();
        Task SaveChangesAsync();

    }
}
