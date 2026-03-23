using Gastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Repositories
{
    public interface ITransacaoRepository
    {
        Task<Transacao> AddAsync(Transacao transacao);
    }
}
