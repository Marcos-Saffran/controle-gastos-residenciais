using Gastos.Api.DTOs;
using Gastos.Infrastructure.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using static Gastos.Api.DTOs.CategoriaDTOs;
using static Gastos.Api.DTOs.PessoaDTOs;
using static Gastos.Api.DTOs.TransacaoDTOs;

namespace Gastos.Tests
{
    public class CategoriaControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CategoriaControllerTests(WebApplicationFactory<Program> factory)
        {
            var sqlite = new SqliteConnection("DataSource=:memory:");
            sqlite.Open();

            var configuredFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.Single(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite(sqlite));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                });
            });

            _client = configuredFactory.CreateClient();
        }

        [Fact]
        public async Task Deve_consultar_totais_por_categoria_e_total_geral()
        {
            var pessoaResponse = await _client.PostAsJsonAsync("/api/pessoas", new PessoaCreateDto("Marcos", 30));
            var pessoaJson = JsonDocument.Parse(await pessoaResponse.Content.ReadAsStringAsync());
            var pessoaId = pessoaJson.RootElement.GetProperty("id").GetInt32();

            var categoria1Response = await _client.PostAsJsonAsync("/api/categorias", new CategoriaCreateDto("Moradia", 1));
            var categoria1Json = JsonDocument.Parse(await categoria1Response.Content.ReadAsStringAsync());
            var categoria1Id = categoria1Json.RootElement.GetProperty("id").GetInt32();

            var categoria2Response = await _client.PostAsJsonAsync("/api/categorias", new CategoriaCreateDto("Salario", 2));
            var categoria2Json = JsonDocument.Parse(await categoria2Response.Content.ReadAsStringAsync());
            var categoria2Id = categoria2Json.RootElement.GetProperty("id").GetInt32();

            await _client.PostAsJsonAsync("/api/transacoes", new TransacaoCreateDto("Aluguel", 90m, 1, categoria1Id, pessoaId));
            await _client.PostAsJsonAsync("/api/transacoes", new TransacaoCreateDto("Bonus", 150m, 2, categoria2Id, pessoaId));
            await _client.PostAsJsonAsync("/api/transacoes", new TransacaoCreateDto("Extra", 10m, 2, categoria2Id, pessoaId));

            var response = await _client.GetAsync("/api/categorias/totais");
            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var categorias = payload.RootElement.GetProperty("categorias");
            Assert.Equal(2, categorias.GetArrayLength());

            var totaisMoradia = categorias.EnumerateArray().First(c => c.GetProperty("categoriaId").GetInt32() == categoria1Id);
            Assert.Equal(0m, totaisMoradia.GetProperty("totalReceitas").GetDecimal());
            Assert.Equal(90m, totaisMoradia.GetProperty("totalDespesas").GetDecimal());
            Assert.Equal(-90m, totaisMoradia.GetProperty("saldo").GetDecimal());

            var totaisSalario = categorias.EnumerateArray().First(c => c.GetProperty("categoriaId").GetInt32() == categoria2Id);
            Assert.Equal(160m, totaisSalario.GetProperty("totalReceitas").GetDecimal());
            Assert.Equal(0m, totaisSalario.GetProperty("totalDespesas").GetDecimal());
            Assert.Equal(160m, totaisSalario.GetProperty("saldo").GetDecimal());

            var totalGeral = payload.RootElement.GetProperty("totalGeral");
            Assert.Equal(160m, totalGeral.GetProperty("totalReceitas").GetDecimal());
            Assert.Equal(90m, totalGeral.GetProperty("totalDespesas").GetDecimal());
            Assert.Equal(70m, totalGeral.GetProperty("saldoLiquido").GetDecimal());
        }
    }
}
