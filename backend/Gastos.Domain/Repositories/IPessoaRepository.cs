using Gastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Repositories
{
    public interface IPessoaRepository
    {
        Task<Pessoa> AddAsync(Pessoa pessoa);
    }
}
