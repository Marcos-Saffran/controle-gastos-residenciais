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
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (nome.Length > 200)
                throw new ArgumentException("Nome deve ter no máximo 200 caracteres.");

            if (idade <= 0)
                throw new ArgumentException("Idade deve ser maior que zero.");

            var pessoa = new Pessoa
            {
                Nome = nome,
                Idade = idade
            };

            return await _pessoaRepository.AddAsync(pessoa);
        }
    }
}
