namespace Gastos.Api.DTOs
{
    public class PessoaDTOs
    {
        public record PessoaCreateDto(string Nome, int Idade);
        public record PessoaUpdateNomeDto(string Nome);
        public record PessoaUpdateIdadeDto(int Idade);

    }
}
