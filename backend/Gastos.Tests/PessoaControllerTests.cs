using Gastos.Api.DTOs;
using Gastos.Infrastructure.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using static Gastos.Api.DTOs.PessoaDTOs;

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
    }
}