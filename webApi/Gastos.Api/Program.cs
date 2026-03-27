using Gastos.Domain.Repositories;
using Gastos.Domain.Services;
using Gastos.Api.Middleware;
using Gastos.Infrastructure.EF;
using Gastos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Banco de Dados (SQLite)
// ==========================================
var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");
Directory.CreateDirectory(dataDirectory);
var databasePath = Path.Combine(dataDirectory, "gastos.db");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));


// ==========================================
// 2. Repositórios (Infrastructure)
// ==========================================
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();


// ==========================================
// 3. Serviços de Domínio (Domain)
// ==========================================
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();


// ==========================================
// 4. Controllers
// ==========================================
builder.Services.AddControllers();


// ==========================================
// 5. Swagger
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ==========================================
// 6. CORS (opcional, mas recomendado)
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();


// ==========================================
// 7. Pipeline
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();
