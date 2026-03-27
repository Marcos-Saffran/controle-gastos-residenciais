using Gastos.Api.DTOs;
using Gastos.Infrastructure.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using static Gastos.Api.DTOs.PessoaDTOs;
using static Gastos.Api.DTOs.CategoriaDTOs;
using static Gastos.Api.DTOs.TransacaoDTOs;

namespace Gastos.Tests
{
    public class PessoaControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PessoaControllerTests(WebApplicationFactory<Program> factory)
        {
            var sqlite = new SqliteConnection("DataSource=:memory:");
            sqlite.Open();

            var configuredFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o DbContext real
                    var descriptor = services.Single(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    services.Remove(descriptor);

                    // Adiciona o DbContext em memória
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite(sqlite));

                    // Cria o banco em memória
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                });
            });

            _client = configuredFactory.CreateClient();
        }

        [Fact]
        public async Task Deve_criar_pessoa()
        {
            var dto = new PessoaCreateDto("Marcos", 30);

            var response = await _client.PostAsJsonAsync("/api/pessoas", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Deve_retornar_bad_request_quando_dados_da_pessoa_forem_invalidos()
        {
            var dto = new PessoaCreateDto("", 0);

            var response = await _client.PostAsJsonAsync("/api/pessoas", dto);
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(problem);
            Assert.Equal("Dados inválidos", problem!.Title);
            Assert.Equal("Nome é obrigatório.", problem.Detail);
        }

        [Fact]
        public async Task Deve_retornar_not_found_quando_alterar_nome_de_pessoa_inexistente()
        {
            var dto = new PessoaUpdateNomeDto("Marcos");

            var response = await _client.PutAsJsonAsync("/api/pessoas/999/nome", dto);
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(problem);
            Assert.Equal("Recurso não encontrado", problem!.Title);
            Assert.Equal("Pessoa não encontrada.", problem.Detail);
        }

        [Fact]
        public async Task Deve_excluir_pessoa_existente()
        {
            var dto = new PessoaCreateDto("Marcos", 30);
            var createResponse = await _client.PostAsJsonAsync("/api/pessoas", dto);
            var payload = await createResponse.Content.ReadAsStringAsync();
            var pessoa = JsonDocument.Parse(payload);
            var pessoaId = pessoa.RootElement.GetProperty("id").GetInt32();

            var deleteResponse = await _client.DeleteAsync($"/api/pessoas/{pessoaId}");
            var getResponse = await _client.GetAsync($"/api/pessoas/{pessoaId}");

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task Deve_excluir_transacoes_da_pessoa_ao_excluir_pessoa()
        {
            var pessoaResponse = await _client.PostAsJsonAsync("/api/pessoas", new PessoaCreateDto("Marcos", 30));
            var pessoaPayload = await pessoaResponse.Content.ReadAsStringAsync();
            var pessoaJson = JsonDocument.Parse(pessoaPayload);
            var pessoaId = pessoaJson.RootElement.GetProperty("id").GetInt32();

            var categoriaResponse = await _client.PostAsJsonAsync("/api/categorias", new CategoriaCreateDto("Moradia", 1));
            var categoriaPayload = await categoriaResponse.Content.ReadAsStringAsync();
            var categoriaJson = JsonDocument.Parse(categoriaPayload);
            var categoriaId = categoriaJson.RootElement.GetProperty("id").GetInt32();

            var transacaoResponse = await _client.PostAsJsonAsync(
                "/api/transacoes",
                new TransacaoCreateDto("Aluguel", 1200m, 1, categoriaId, pessoaId));

            var transacaoPayload = await transacaoResponse.Content.ReadAsStringAsync();
            var transacaoJson = JsonDocument.Parse(transacaoPayload);
            var transacaoId = transacaoJson.RootElement.GetProperty("id").GetInt32();

            var deletePessoaResponse = await _client.DeleteAsync($"/api/pessoas/{pessoaId}");
            var obterTransacaoResponse = await _client.GetAsync($"/api/transacoes/{transacaoId}");

            Assert.Equal(HttpStatusCode.NoContent, deletePessoaResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, obterTransacaoResponse.StatusCode);
        }

        [Fact]
        public async Task Deve_consultar_totais_por_pessoa_e_total_geral()
        {
            var pessoa1Response = await _client.PostAsJsonAsync("/api/pessoas", new PessoaCreateDto("Marcos", 30));
            var pessoa1Json = JsonDocument.Parse(await pessoa1Response.Content.ReadAsStringAsync());
            var pessoa1Id = pessoa1Json.RootElement.GetProperty("id").GetInt32();

            var pessoa2Response = await _client.PostAsJsonAsync("/api/pessoas", new PessoaCreateDto("Ana", 28));
            var pessoa2Json = JsonDocument.Parse(await pessoa2Response.Content.ReadAsStringAsync());
            var pessoa2Id = pessoa2Json.RootElement.GetProperty("id").GetInt32();

            var categoriaResponse = await _client.PostAsJsonAsync("/api/categorias", new CategoriaCreateDto("Geral", 3));
            var categoriaJson = JsonDocument.Parse(await categoriaResponse.Content.ReadAsStringAsync());
            var categoriaId = categoriaJson.RootElement.GetProperty("id").GetInt32();

            await _client.PostAsJsonAsync("/api/transacoes", new TransacaoCreateDto("Salário", 100m, 2, categoriaId, pessoa1Id));
            await _client.PostAsJsonAsync("/api/transacoes", new TransacaoCreateDto("Mercado", 40m, 1, categoriaId, pessoa1Id));
            await _client.PostAsJsonAsync("/api/transacoes", new TransacaoCreateDto("Transporte", 20m, 1, categoriaId, pessoa2Id));

            var response = await _client.GetAsync("/api/pessoas/totais");
            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var pessoas = payload.RootElement.GetProperty("pessoas");
            Assert.Equal(2, pessoas.GetArrayLength());

            var totaisMarcos = pessoas.EnumerateArray().First(p => p.GetProperty("pessoaId").GetInt32() == pessoa1Id);
            Assert.Equal(100m, totaisMarcos.GetProperty("totalReceitas").GetDecimal());
            Assert.Equal(40m, totaisMarcos.GetProperty("totalDespesas").GetDecimal());
            Assert.Equal(60m, totaisMarcos.GetProperty("saldo").GetDecimal());

            var totaisAna = pessoas.EnumerateArray().First(p => p.GetProperty("pessoaId").GetInt32() == pessoa2Id);
            Assert.Equal(0m, totaisAna.GetProperty("totalReceitas").GetDecimal());
            Assert.Equal(20m, totaisAna.GetProperty("totalDespesas").GetDecimal());
            Assert.Equal(-20m, totaisAna.GetProperty("saldo").GetDecimal());

            var totalGeral = payload.RootElement.GetProperty("totalGeral");
            Assert.Equal(100m, totalGeral.GetProperty("totalReceitas").GetDecimal());
            Assert.Equal(60m, totalGeral.GetProperty("totalDespesas").GetDecimal());
            Assert.Equal(40m, totalGeral.GetProperty("saldoLiquido").GetDecimal());
        }
    }
}