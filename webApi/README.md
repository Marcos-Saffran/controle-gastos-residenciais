# Backend (Web API) - Controle de Gastos

Este diretório tem toda a parte de backend da aplicação.

## Projetos dentro de webApi

- `Gastos.Api`: endpoints HTTP (controllers)
- `Gastos.Domain`: entidades e serviços
- `Gastos.Infrastructure`: EF Core, contexto e repositórios
- `Gastos.Tests`: testes unitários e de integração de controllers

## Tecnologias

- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- xUnit + Moq

## Pré-requisitos

- .NET SDK 10
- Ferramenta `dotnet-ef` (se não tiver instalada)

Para instalar `dotnet-ef`:

```bash
dotnet tool install --global dotnet-ef
```

## Como executar a API

Na pasta `webApi/Gastos.Api`:

```bash
dotnet restore
dotnet ef database update --project ../Gastos.Infrastructure --startup-project .
dotnet run
```

Se aparecer erro do SQLite "unable to open database file", rode antes:

```bash
mkdir Data
```

URLs da API:
- http://localhost:5000
- https://localhost:5001

Swagger:
- https://localhost:5001/swagger

## Rodar testes

Na pasta `webApi`:

```bash
dotnet test Gastos.Tests/Gastos.Tests.csproj
```

## Observações rápidas

- CORS está liberado para facilitar o teste com o frontend.
- O banco SQLite fica no caminho `Data/gastos.db` da API.