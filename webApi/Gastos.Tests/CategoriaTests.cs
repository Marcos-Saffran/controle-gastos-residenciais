using Gastos.Domain.Entities;

namespace Gastos.Tests
{
    public class CategoriaTests
    {
        [Fact]
        public void Nao_deve_criar_categoria_com_descricao_vazia()
        {
            Assert.Throws<ArgumentException>(() =>
                new Categoria("", FinalidadeCategoria.Despesa));
        }

        [Fact]
        public void Nao_deve_criar_categoria_com_descricao_maior_que_400_caracteres()
        {
            var descGrande = new string('A', 401);

            Assert.Throws<ArgumentException>(() =>
                new Categoria(descGrande, FinalidadeCategoria.Receita));
        }

        [Fact]
        public void Nao_deve_criar_categoria_com_finalidade_invalida()
        {
            var finalidadeInvalida = (FinalidadeCategoria)0;

            Assert.Throws<ArgumentException>(() =>
                new Categoria("Transporte", finalidadeInvalida));
        }

    }
}
