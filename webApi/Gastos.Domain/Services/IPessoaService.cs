using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Services
{
    public interface IPessoaService
    {
        Task AlterarNomeAsync(int pessoaId, string novoNome);
        Task AlterarIdadeAsync(int pessoaId, int novaIdade);
        Task ExcluirPessoaAsync(int pessoaId);
    }
}
