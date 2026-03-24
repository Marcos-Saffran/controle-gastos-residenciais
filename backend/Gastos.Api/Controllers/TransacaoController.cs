using Gastos.Api.DTOs;
using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using static Gastos.Api.DTOs.TransacaoDTOs;

namespace Gastos.Api.Controllers
{

    [ApiController]
    [Route("api/transacoes")]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoRepository _repo;
        private readonly ITransacaoService _service;

        public TransacaoController(ITransacaoRepository repo, ITransacaoService service)
        {
            _repo = repo;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(TransacaoCreateDto dto)
        {
            var transacao = await _service.CriarTransacaoAsync(
                dto.Descricao,
                dto.Valor,
                (TipoTransacao)dto.Tipo,
                dto.CategoriaId,
                dto.PessoaId
            );

            return CreatedAtAction(nameof(ObterPorId), new { id = transacao.Id }, transacao);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var transacao = await _repo.ObterPorIdAsync(id);
            return transacao is null ? NotFound() : Ok(transacao);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _repo.ListarAsync());
        }

        [HttpPut("{id}/descricao")]
        public async Task<IActionResult> AlterarDescricao(int id, TransacaoUpdateDescricaoDto dto)
        {
            await _service.AlterarDescricaoAsync(id, dto.Descricao);
            return NoContent();
        }

        [HttpPut("{id}/valor")]
        public async Task<IActionResult> AlterarValor(int id, TransacaoUpdateValorDto dto)
        {
            await _service.AlterarValorAsync(id, dto.Valor);
            return NoContent();
        }

        [HttpPut("{id}/tipo")]
        public async Task<IActionResult> AlterarTipo(int id, TransacaoUpdateTipoDto dto)
        {
            await _service.AlterarTipoAsync(id, (TipoTransacao)dto.Tipo);
            return NoContent();
        }
    }
}