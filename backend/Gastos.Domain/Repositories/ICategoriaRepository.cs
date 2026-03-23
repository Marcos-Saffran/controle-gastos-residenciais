using Gastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AddAsync(Categoria categoria);
        Task<Categoria?> ObterPorIdAsync(int categoriaId);
    }
}
