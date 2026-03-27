using Gastos.Domain.Entities;

namespace Gastos.Tests
{
    public class PessoaTests
    {

        [Fact]
        public void Nao_deve_criar_pessoa_com_nome_vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Pessoa("", 25));
        }

        [Fact]
        public void Nao_deve_criar_pessoa_com_nome_maior_que_200_caracteres()
        {
            var nomeGrande = new string('A', 201);

            Assert.Throws<ArgumentException>(() =>
                new Pessoa(nomeGrande, 25));
        }

        [Fact]
        public void Nao_deve_criar_pessoa_com_idade_invalida()
        {
            Assert.Throws<ArgumentException>(() =>
                new Pessoa("Marcos", 0));
        }

    }
}