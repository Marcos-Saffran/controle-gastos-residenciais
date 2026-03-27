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
            var transacaoRepo = new Mock<ITransacaoRepository>();
            var pessoa = new Pessoa("Marcos", 30);

            repo.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(pessoa);

            var service = new PessoaService(repo.Object, transacaoRepo.Object);

            await service.AlterarNomeAsync(1, "Marcos Silva");

            Assert.Equal("Marcos Silva", pessoa.Nome);
        }

        [Fact]
        public async Task Nao_deve_alterar_nome_quando_pessoa_nao_existir()
        {
            var repo = new Mock<IPessoaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            repo.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync((Pessoa?)null);

            var service = new PessoaService(repo.Object, transacaoRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AlterarNomeAsync(1, "Novo Nome"));
        }

        [Fact]
        public async Task Deve_excluir_pessoa_existente()
        {
            var repo = new Mock<IPessoaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();
            var pessoa = new Pessoa("Marcos", 30);

            repo.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(pessoa);

            var service = new PessoaService(repo.Object, transacaoRepo.Object);

            await service.ExcluirPessoaAsync(1);

            transacaoRepo.Verify(r => r.DeleteByPessoaIdAsync(1), Times.Once);
            repo.Verify(r => r.DeleteAsync(pessoa), Times.Once);
        }

        [Fact]
        public async Task Nao_deve_excluir_pessoa_quando_nao_existir()
        {
            var repo = new Mock<IPessoaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            repo.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync((Pessoa?)null);

            var service = new PessoaService(repo.Object, transacaoRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.ExcluirPessoaAsync(1));
        }



    }
}