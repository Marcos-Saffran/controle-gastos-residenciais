using Gastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Tests
{
    public class TransacaoTests
    {
        [Fact]
        public void Nao_deve_criar_transacao_com_valor_menor_ou_igual_a_zero()
        {
            Assert.Throws<ArgumentException>(() =>
                new Transacao("Compra", 0, TipoTransacao.Despesa, 1, 1));
        }

        [Fact]
        public void Nao_deve_criar_transacao_com_descricao_vazia()
        {
            Assert.Throws<ArgumentException>(() =>
                new Transacao("", 100, TipoTransacao.Despesa, 1, 1));
        }

        [Fact]
        public void Nao_deve_criar_transacao_com_descricao_maior_que_400_caracteres()
        {
            var descGrande = new string('A', 401);

            Assert.Throws<ArgumentException>(() =>
                new Transacao(descGrande, 100, TipoTransacao.Despesa, 1, 1));
        }

        [Fact]
        public void Nao_deve_criar_transacao_com_tipo_invalido()
        {
            var tipoInvalido = (TipoTransacao)0;

            Assert.Throws<ArgumentException>(() =>
                new Transacao("Compra", 100, tipoInvalido, 1, 1));
        }

        [Fact]
        public void Deve_criar_transacao_valida()
        {
            var transacao = new Transacao("Mercado", 150, TipoTransacao.Despesa, 1, 1);

            Assert.Equal("Mercado", transacao.Descricao);
            Assert.Equal(150, transacao.Valor);
            Assert.Equal(TipoTransacao.Despesa, transacao.Tipo);
            Assert.Equal(1, transacao.CategoriaId);
            Assert.Equal(1, transacao.PessoaId);
        }

        [Fact]
        public void Deve_alterar_descricao_com_sucesso()
        {
            var transacao = new Transacao("Mercado", 150, TipoTransacao.Despesa, 1, 1);

            transacao.SetDescricao("Supermercado");

            Assert.Equal("Supermercado", transacao.Descricao);
        }

        [Fact]
        public void Nao_deve_alterar_descricao_para_invalida()
        {
            var transacao = new Transacao("Mercado", 150, TipoTransacao.Despesa, 1, 1);

            Assert.Throws<ArgumentException>(() =>
                transacao.SetDescricao(""));
        }



    }
}
