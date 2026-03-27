namespace Gastos.Api.DTOs
{
    public class CategoriaDTOs
    {
        public record CategoriaCreateDto(string Descricao, int Finalidade);
        public record CategoriaUpdateDescricaoDto(string Descricao);
        public record CategoriaUpdateFinalidadeDto(int Finalidade);

        public record CategoriaTotaisDto(
            int CategoriaId,
            string Descricao,
            decimal TotalReceitas,
            decimal TotalDespesas,
            decimal Saldo);

        public record CategoriaTotaisGeraisDto(
            decimal TotalReceitas,
            decimal TotalDespesas,
            decimal SaldoLiquido);

        public record CategoriaTotaisConsultaResponseDto(
            List<CategoriaTotaisDto> Categorias,
            CategoriaTotaisGeraisDto TotalGeral);

    }
}
