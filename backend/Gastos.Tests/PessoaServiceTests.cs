using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Moq;

namespace Gastos.Tests
{
    public class PessoaServiceTests
    {
        [Fact]
        public async Task Deve_alterar_nome_da_pessoa()
        {
            var repo = new Mock<IPessoaRepository>();
            var pessoa = new Pessoa("Marcos", 30);

            repo.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(pessoa);

            var service = new PessoaService(repo.Object);

            await service.AlterarNomeAsync(1, "Marcos Silva");

            Assert.Equal("Marcos Silva", pessoa.Nome);
        }

        [Fact]
        public async Task Nao_deve_alterar_nome_quando_pessoa_nao_existir()
        {
            var repo = new Mock<IPessoaRepository>();

            repo.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync((Pessoa?)null);

            var service = new PessoaService(repo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AlterarNomeAsync(1, "Novo Nome"));
        }



    }
}