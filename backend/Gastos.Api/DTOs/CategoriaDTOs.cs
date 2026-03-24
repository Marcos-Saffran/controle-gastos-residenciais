namespace Gastos.Api.DTOs
{
    public class CategoriaDTOs
    {
        public record CategoriaCreateDto(string Descricao, int Finalidade);
        public record CategoriaUpdateDescricaoDto(string Descricao);
        public record CategoriaUpdateFinalidadeDto(int Finalidade);

    }
}
