using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;

namespace Gastos.Domain.Services
{
    public class TransacaoService : ITransacaoService
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

        public async Task AlterarDescricaoAsync(int transacaoId, string novaDescricao)
        {
            var transacao = await _transacaoRepo.ObterPorIdAsync(transacaoId)
                             ?? throw new InvalidOperationException("Transação não encontrada.");

            transacao.SetDescricao(novaDescricao);

            await _transacaoRepo.SaveChangesAsync();
        }

        public Task AlterarTipoAsync(int transacaoId, TipoTransacao novoTipo)
        {
            // todo: implementar regras de validação para alteração de tipo (ex: categoria aceita o novo tipo?)
            throw new NotImplementedException();
        }

        public Task AlterarValorAsync(int transacaoId, decimal novoValor)
        {
            // todo: implementar regras de validação para alteração de valor (ex: valor não pode ser negativo)
            throw new NotImplementedException();
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
