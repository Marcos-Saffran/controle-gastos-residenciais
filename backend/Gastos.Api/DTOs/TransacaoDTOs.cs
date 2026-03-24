namespace Gastos.Api.DTOs
{
    public class TransacaoDTOs
    {
    public record TransacaoCreateDto(
        string Descricao,
        decimal Valor,
        int Tipo,
        int CategoriaId,
        int PessoaId
    );

    public record TransacaoUpdateDescricaoDto(string Descricao);
    public record TransacaoUpdateValorDto(decimal Valor);
    public record TransacaoUpdateTipoDto(int Tipo);

    }
}
