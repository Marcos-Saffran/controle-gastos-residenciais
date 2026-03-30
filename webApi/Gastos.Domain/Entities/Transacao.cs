namespace Gastos.Domain.Entities;

public enum TipoTransacao
{
    Despesa = 1,
    Receita = 2
}

public class Transacao
{
    public int Id { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public TipoTransacao Tipo { get; private set; }

    public int CategoriaId { get; private set; }
    public int PessoaId { get; private set; }

    protected Transacao() { }

    public Transacao(
        string descricao,
        decimal valor,
        TipoTransacao tipo,
        int categoriaId,
        int pessoaId)
    {
        SetDescricao(descricao);
        SetValor(valor);
        SetTipo(tipo);

        if (categoriaId <= 0)
            throw new ArgumentException("CategoriaId inválido.");

        if (pessoaId <= 0)
            throw new ArgumentException("PessoaId inválido.");

        CategoriaId = categoriaId;
        PessoaId = pessoaId;
    }

    public void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória.");

        if (descricao.Length > 400)
            throw new ArgumentException("Descrição deve ter no máximo 400 caracteres.");

        Descricao = descricao;
    }

    public void SetValor(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero.");

        Valor = valor;
    }

    public void SetTipo(TipoTransacao tipo)
    {
        if (!Enum.IsDefined(tipo))
            throw new ArgumentException("Tipo inválido.");

        Tipo = tipo;
    }
}