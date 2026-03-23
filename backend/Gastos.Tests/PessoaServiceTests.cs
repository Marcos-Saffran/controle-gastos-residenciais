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

    [Fact]
    public async Task Nao_deve_criar_pessoa_com_nome_vazio()
    {
        // Arrange
        var repoMock = new Mock<IPessoaRepository>();
        var service = new PessoaService(repoMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CriarPessoaAsync("", 25));
    }

    [Fact]
    public async Task Nao_deve_criar_pessoa_com_nome_maior_que_200_caracteres()
    {
        // Arrange
        var repoMock = new Mock<IPessoaRepository>();
        var service = new PessoaService(repoMock.Object);

        var nomeGrande = new string('A', 201);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CriarPessoaAsync(nomeGrande, 25));
    }

    [Fact]
    public async Task Nao_deve_criar_pessoa_com_idade_menor_ou_igual_a_zero()
    {
        // Arrange
        var repoMock = new Mock<IPessoaRepository>();
        var service = new PessoaService(repoMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CriarPessoaAsync("Marcos", 0));
    }
}
