using Gastos.Api.DTOs;
using Gastos.Domain.Entities;
using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using static Gastos.Api.DTOs.PessoaDTOs;

namespace Gastos.Api.Controllers
{


    [ApiController]
    [Route("api/pessoas")]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaRepository _repo;
        private readonly ITransacaoRepository _transacaoRepo;
        private readonly IPessoaService _service;

        public PessoaController(IPessoaRepository repo, ITransacaoRepository transacaoRepo, IPessoaService service)
        {
            _repo = repo;
            _transacaoRepo = transacaoRepo;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(PessoaCreateDto dto)
        {
            var pessoa = new Pessoa(dto.Nome, dto.Idade);
            await _repo.AddAsync(pessoa);
            return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var pessoa = await _repo.ObterPorIdAsync(id);
            return pessoa is null ? NotFound() : Ok(pessoa);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _repo.ListarAsync());
        }

        [HttpGet("totais")]
        public async Task<IActionResult> ConsultarTotaisPorPessoa()
        {
            var pessoas = await _repo.ListarAsync();
            var transacoes = await _transacaoRepo.ListarAsync();

            var totaisPorPessoa = pessoas.Select(pessoa =>
            {
                var totalReceitas = transacoes
                    .Where(t => t.PessoaId == pessoa.Id && t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                var totalDespesas = transacoes
                    .Where(t => t.PessoaId == pessoa.Id && t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                return new PessoaTotaisDto(
                    pessoa.Id,
                    pessoa.Nome,
                    totalReceitas,
                    totalDespesas,
                    totalReceitas - totalDespesas);
            }).ToList();

            var totalGeralReceitas = totaisPorPessoa.Sum(item => item.TotalReceitas);
            var totalGeralDespesas = totaisPorPessoa.Sum(item => item.TotalDespesas);

            var response = new PessoaTotaisConsultaResponseDto(
                totaisPorPessoa,
                new TotaisGeraisDto(
                    totalGeralReceitas,
                    totalGeralDespesas,
                    totalGeralReceitas - totalGeralDespesas));

            return Ok(response);
        }

        [HttpPut("{id}/nome")]
        public async Task<IActionResult> AlterarNome(int id, PessoaUpdateNomeDto dto)
        {
            await _service.AlterarNomeAsync(id, dto.Nome);
            return NoContent();
        }

        [HttpPut("{id}/idade")]
        public async Task<IActionResult> AlterarIdade(int id, PessoaUpdateIdadeDto dto)
        {
            await _service.AlterarIdadeAsync(id, dto.Idade);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            await _service.ExcluirPessoaAsync(id);
            return NoContent();
        }
    }
}