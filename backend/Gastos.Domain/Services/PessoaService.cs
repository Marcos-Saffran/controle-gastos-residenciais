using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Services
{
    public class PessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        public async Task<Pessoa> CriarPessoaAsync(string nome, int idade)
        {
            var pessoa = new Pessoa
            {
                Nome = nome,
                Idade = idade
            };
            return await _pessoaRepository.AddAsync(pessoa);
        }
    }
}
