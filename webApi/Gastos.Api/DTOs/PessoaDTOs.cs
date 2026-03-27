namespace Gastos.Api.DTOs
{
    public class PessoaDTOs
    {
        public record PessoaCreateDto(string Nome, int Idade);
        public record PessoaUpdateNomeDto(string Nome);
        public record PessoaUpdateIdadeDto(int Idade);

        public record PessoaTotaisDto(
            int PessoaId,
            string Nome,
            decimal TotalReceitas,
            decimal TotalDespesas,
            decimal Saldo);

        public record TotaisGeraisDto(
            decimal TotalReceitas,
            decimal TotalDespesas,
            decimal SaldoLiquido);

        public record PessoaTotaisConsultaResponseDto(
            List<PessoaTotaisDto> Pessoas,
            TotaisGeraisDto TotalGeral);

    }
}
