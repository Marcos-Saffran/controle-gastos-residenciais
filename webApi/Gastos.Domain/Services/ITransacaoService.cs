using Gastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Services
{
    public interface ITransacaoService
    {
        Task<Transacao> CriarTransacaoAsync(
            string descricao,
            decimal valor,
            TipoTransacao tipo,
            int categoriaId,
            int pessoaId);

        Task AlterarDescricaoAsync(int transacaoId, string novaDescricao);
        Task AlterarValorAsync(int transacaoId, decimal novoValor);
        Task AlterarTipoAsync(int transacaoId, TipoTransacao novoTipo);
    }
}
