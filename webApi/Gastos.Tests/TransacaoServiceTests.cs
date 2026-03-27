using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Moq;

namespace Gastos.Tests
{
    public class TransacaoServiceTests
    {
        [Fact]
        public async Task Nao_deve_criar_transacao_quando_pessoa_nao_existir()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            pessoaRepo.Setup(r => r.ObterPorIdAsync(1))
                      .ReturnsAsync((Pessoa?)null);

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CriarTransacaoAsync("Mercado", 100, TipoTransacao.Despesa, 1, 1));
        }

        [Fact]
        public async Task Nao_deve_criar_transacao_quando_categoria_nao_existir()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            pessoaRepo.Setup(r => r.ObterPorIdAsync(1))
                      .ReturnsAsync(new Pessoa("Marcos", 30));

            categoriaRepo.Setup(r => r.ObterPorIdAsync(1))
                         .ReturnsAsync((Categoria?)null);

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CriarTransacaoAsync("Mercado", 100, TipoTransacao.Despesa, 1, 1));
        }

        [Fact]
        public async Task Menor_de_idade_nao_pode_registrar_receita()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            pessoaRepo.Setup(r => r.ObterPorIdAsync(1))
                      .ReturnsAsync(new Pessoa("João", 16)); // menor

            categoriaRepo.Setup(r => r.ObterPorIdAsync(1))
                         .ReturnsAsync(new Categoria("Salário", FinalidadeCategoria.Receita));

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CriarTransacaoAsync("Salário", 500, TipoTransacao.Receita, 1, 1));
        }

        [Fact]
        public async Task Categoria_deve_ser_compativel_com_tipo_despesa()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            pessoaRepo.Setup(r => r.ObterPorIdAsync(1))
                      .ReturnsAsync(new Pessoa("Marcos", 30));

            categoriaRepo.Setup(r => r.ObterPorIdAsync(1))
                         .ReturnsAsync(new Categoria("Aluguel", FinalidadeCategoria.Despesa));

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CriarTransacaoAsync("Salário", 500, TipoTransacao.Receita, 1, 1));
        }

        [Fact]
        public async Task Deve_criar_transacao_valida()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            pessoaRepo.Setup(r => r.ObterPorIdAsync(1))
                      .ReturnsAsync(new Pessoa("Marcos", 30));

            categoriaRepo.Setup(r => r.ObterPorIdAsync(1))
                         .ReturnsAsync(new Categoria("Alimentação", FinalidadeCategoria.Ambas));

            transacaoRepo.Setup(r => r.AddAsync(It.IsAny<Transacao>()))
                         .ReturnsAsync((Transacao t) =>
                         {
                             // simula ID gerado automaticamente
                             typeof(Transacao)
                                 .GetProperty("Id")!
                                 .SetValue(t, 10);

                             return t;
                         });

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act
            var transacao = await service.CriarTransacaoAsync("Mercado", 150, TipoTransacao.Despesa, 1, 1);

            // Assert
            Assert.Equal(10, transacao.Id);
            Assert.Equal("Mercado", transacao.Descricao);
            Assert.Equal(150, transacao.Valor);
        }

        [Fact]
        public async Task Categoria_de_receita_nao_aceita_despesa()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            pessoaRepo.Setup(r => r.ObterPorIdAsync(1))
                      .ReturnsAsync(new Pessoa("Marcos", 30));

            categoriaRepo.Setup(r => r.ObterPorIdAsync(1))
                         .ReturnsAsync(new Categoria("Salário", FinalidadeCategoria.Receita));

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CriarTransacaoAsync("Aluguel", 1000, TipoTransacao.Despesa, 1, 1));
        }

        [Fact]
        public async Task Categoria_ambas_aceita_despesa_e_receita()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            pessoaRepo.Setup(r => r.ObterPorIdAsync(1))
                      .ReturnsAsync(new Pessoa("Marcos", 30));

            categoriaRepo.Setup(r => r.ObterPorIdAsync(1))
                         .ReturnsAsync(new Categoria("Geral", FinalidadeCategoria.Ambas));

            transacaoRepo.Setup(r => r.AddAsync(It.IsAny<Transacao>()))
                         .ReturnsAsync((Transacao t) => t);

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act
            var transacao = await service.CriarTransacaoAsync("Teste", 50, TipoTransacao.Receita, 1, 1);

            // Assert
            Assert.Equal("Teste", transacao.Descricao);
        }

        [Fact]
        public async Task Deve_alterar_descricao_da_transacao()
        {
            // Arrange
            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            var transacao = new Transacao("Mercado", 150, TipoTransacao.Despesa, 1, 1);

            transacaoRepo.Setup(r => r.ObterPorIdAsync(10))
                         .ReturnsAsync(transacao);

            var service = new TransacaoService(
                pessoaRepo.Object,
                categoriaRepo.Object,
                transacaoRepo.Object
            );

            // Act
            await service.AlterarDescricaoAsync(10, "Supermercado");

            // Assert
            Assert.Equal("Supermercado", transacao.Descricao);
        }



    }
}
