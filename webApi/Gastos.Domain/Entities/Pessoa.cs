namespace Gastos.Domain.Entities;

public class Pessoa
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public int Idade { get; private set; }

    // Construtor protegido para o EF Core
    protected Pessoa() { }

    public Pessoa(string nome, int idade)
    {
        SetNome(nome);
        SetIdade(idade);
    }

    public void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");

        if (nome.Length > 200)
            throw new ArgumentException("Nome deve ter no máximo 200 caracteres.");

        Nome = nome;
    }

    public void SetIdade(int idade)
    {
        if (idade <= 0)
            throw new ArgumentException("Idade deve ser maior que zero.");

        Idade = idade;
    }
}