using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Moq;


public class PessoaServiceTests
{
    [Fact]
    public async Task Deve_criar_pessoa_com_id_gerado_automaticamente()
    {
        // Arrange
        var repoMock = new Mock<IPessoaRepository>();

        // Simula que o repositório vai gerar ID = 1
        repoMock.Setup(r => r.AddAsync(It.IsAny<Pessoa>()))
                .ReturnsAsync((Pessoa p) =>
                {
                    p.Id = 1;
                    return p;
                });

        var service = new PessoaService(repoMock.Object);

        // Act
        var pessoaCriada = await service.CriarPessoaAsync("Marcos", 30);

        // Assert
        Assert.Equal(1, pessoaCriada.Id);
        Assert.Equal("Marcos", pessoaCriada.Nome);
        Assert.Equal(30, pessoaCriada.Idade);
    }

}
