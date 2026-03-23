namespace Gastos.Domain.Entities;

public enum FinalidadeCategoria
{
    Despesa = 1,
    Receita = 2,
    Ambas = 3
}

public class Categoria
{
    public int Id { get; private set; }
    public string Descricao { get; private set; }
    public FinalidadeCategoria Finalidade { get; private set; }

    protected Categoria() { }

    public Categoria(string descricao, FinalidadeCategoria finalidade)
    {
        SetDescricao(descricao);
        SetFinalidade(finalidade);
    }

    public void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória.");

        if (descricao.Length > 400)
            throw new ArgumentException("Descrição deve ter no máximo 400 caracteres.");

        Descricao = descricao;
    }

    public void SetFinalidade(FinalidadeCategoria finalidade)
    {
        if (!Enum.IsDefined(typeof(FinalidadeCategoria), finalidade))
            throw new ArgumentException("Finalidade inválida.");

        Finalidade = finalidade;
    }
}