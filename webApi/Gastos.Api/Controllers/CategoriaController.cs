using Gastos.Api.DTOs;
using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using static Gastos.Api.DTOs.CategoriaDTOs;

namespace Gastos.Api.Controllers
{

    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repo;
        private readonly ITransacaoRepository _transacaoRepo;
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaRepository repo, ITransacaoRepository transacaoRepo, ICategoriaService service)
        {
            _repo = repo;
            _transacaoRepo = transacaoRepo;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CategoriaCreateDto dto)
        {
            var categoria = new Categoria(dto.Descricao, (FinalidadeCategoria)dto.Finalidade);
            await _repo.AddAsync(categoria);
            return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, categoria);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var categoria = await _repo.ObterPorIdAsync(id);
            return categoria is null ? NotFound() : Ok(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _repo.ListarAsync());
        }

        [HttpGet("totais")]
        public async Task<IActionResult> ConsultarTotaisPorCategoria()
        {
            var categorias = await _repo.ListarAsync();
            var transacoes = await _transacaoRepo.ListarAsync();

            var totaisPorCategoria = categorias.Select(categoria =>
            {
                var totalReceitas = transacoes
                    .Where(t => t.CategoriaId == categoria.Id && t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                var totalDespesas = transacoes
                    .Where(t => t.CategoriaId == categoria.Id && t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                return new CategoriaTotaisDto(
                    categoria.Id,
                    categoria.Descricao,
                    totalReceitas,
                    totalDespesas,
                    totalReceitas - totalDespesas);
            }).ToList();

            var totalGeralReceitas = totaisPorCategoria.Sum(item => item.TotalReceitas);
            var totalGeralDespesas = totaisPorCategoria.Sum(item => item.TotalDespesas);

            var response = new CategoriaTotaisConsultaResponseDto(
                totaisPorCategoria,
                new CategoriaTotaisGeraisDto(
                    totalGeralReceitas,
                    totalGeralDespesas,
                    totalGeralReceitas - totalGeralDespesas));

            return Ok(response);
        }

        [HttpPut("{id}/descricao")]
        public async Task<IActionResult> AlterarDescricao(int id, CategoriaUpdateDescricaoDto dto)
        {
            await _service.AlterarDescricaoAsync(id, dto.Descricao);
            return NoContent();
        }

        [HttpPut("{id}/finalidade")]
        public async Task<IActionResult> AlterarFinalidade(int id, CategoriaUpdateFinalidadeDto dto)
        {
            await _service.AlterarFinalidadeAsync(id, (FinalidadeCategoria)dto.Finalidade);
            return NoContent();
        }
    }
}