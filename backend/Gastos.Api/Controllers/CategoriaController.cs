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
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaRepository repo, ICategoriaService service)
        {
            _repo = repo;
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