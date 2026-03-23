using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
