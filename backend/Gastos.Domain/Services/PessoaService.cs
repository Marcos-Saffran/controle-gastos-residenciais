using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;

namespace Gastos.Domain.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
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

    }
}
