using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;

namespace Gastos.Domain.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ITransacaoRepository _transacaoRepository;

        public PessoaService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository)
        {
            _pessoaRepository = pessoaRepository;
            _transacaoRepository = transacaoRepository;
        }

        public async Task<Pessoa> CriarPessoaAsync(string nome, int idade)
        {
            var pessoa = new Pessoa(nome, idade);

            return await _pessoaRepository.AddAsync(pessoa);
        }

        public async Task AlterarNomeAsync(int pessoaId, string novoNome)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(pessoaId)
                         ?? throw new InvalidOperationException("Pessoa não encontrada.");

            pessoa.SetNome(novoNome);

            await _pessoaRepository.SaveChangesAsync();
        }

        public async Task AlterarIdadeAsync(int pessoaId, int novaIdade)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(pessoaId)
                         ?? throw new InvalidOperationException("Pessoa não encontrada.");

            pessoa.SetIdade(novaIdade);

            await _pessoaRepository.SaveChangesAsync();
        }

        public async Task ExcluirPessoaAsync(int pessoaId)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(pessoaId)
                         ?? throw new InvalidOperationException("Pessoa não encontrada.");

            await _transacaoRepository.DeleteByPessoaIdAsync(pessoaId);
            await _pessoaRepository.DeleteAsync(pessoa);
        }

    }
}
