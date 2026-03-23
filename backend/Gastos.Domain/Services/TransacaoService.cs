using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Services
{
    public class TransacaoService
    {
        private readonly IPessoaRepository _pessoaRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly ITransacaoRepository _transacaoRepo;

        public TransacaoService(
            IPessoaRepository pessoaRepo,
            ICategoriaRepository categoriaRepo,
            ITransacaoRepository transacaoRepo)
        {
            _pessoaRepo = pessoaRepo;
            _categoriaRepo = categoriaRepo;
            _transacaoRepo = transacaoRepo;
        }

        public async Task<Transacao> CriarTransacaoAsync(
    string descricao,
    decimal valor,
    TipoTransacao tipo,
    int categoriaId,
    int pessoaId)
        {
            var pessoa = await _pessoaRepo.ObterPorIdAsync(pessoaId)
                         ?? throw new InvalidOperationException("Pessoa não encontrada.");

            var categoria = await _categoriaRepo.ObterPorIdAsync(categoriaId)
                           ?? throw new InvalidOperationException("Categoria não encontrada.");

            // Regra: menor de idade só pode despesa
            if (pessoa.Idade < 18 && tipo == TipoTransacao.Receita)
                throw new InvalidOperationException("Menores de idade só podem registrar despesas.");

            // Regra: categoria deve aceitar o tipo
            var aceitaDespesa = categoria.Finalidade is FinalidadeCategoria.Despesa or FinalidadeCategoria.Ambas;
            var aceitaReceita = categoria.Finalidade is FinalidadeCategoria.Receita or FinalidadeCategoria.Ambas;

            if (tipo == TipoTransacao.Despesa && !aceitaDespesa)
                throw new InvalidOperationException("Categoria não aceita despesas.");

            if (tipo == TipoTransacao.Receita && !aceitaReceita)
                throw new InvalidOperationException("Categoria não aceita receitas.");

            var transacao = new Transacao(descricao, valor, tipo, categoriaId, pessoaId);

            return await _transacaoRepo.AddAsync(transacao);
        }
    }
}
