# Controle de Gastos Residenciais

Projeto que fiz para o teste de vaga de desenvolvedor júnior.

A ideia da aplicação é cadastrar:
- pessoas
- categorias de gastos
- transações (entradas e saídas)

Depois disso, visualizar e organizar os gastos da casa de forma simples.

## Tecnologias usadas

- Backend: ASP.NET Core Web API (.NET 10)
- Banco: SQLite + Entity Framework Core
- Frontend: React + TypeScript
- Testes backend: xUnit + Moq

## Estrutura do projeto

- `frontend/gastos-frontend`: aplicação React
- `webApi/Gastos.Api`: API principal
- `webApi/Gastos.Domain`: regras de negócio e entidades
- `webApi/Gastos.Infrastructure`: acesso a dados e EF Core
- `webApi/Gastos.Tests`: testes unitários e de controller

## Pré-requisitos

- .NET SDK 10
- Node.js (recomendado 18+)
- npm

## Como rodar o projeto completo

### 1. Subir a API

No terminal:

```bash
cd webApi/Gastos.Api
dotnet restore
dotnet ef database update --project ../Gastos.Infrastructure --startup-project .
dotnet run
```

API em:
- http://localhost:5000
- https://localhost:5001

Swagger:
- https://localhost:5001/swagger

### 2. Subir o frontend

Em outro terminal:

```bash
cd frontend/gastos-frontend
npm install
npm start
```

Frontend em:
- http://localhost:3000

Observação: o frontend está configurado para consumir a API em `http://localhost:5000/api`.

## Rodar testes do backend

```bash
cd webApi
dotnet test Gastos.Tests/Gastos.Tests.csproj
```

## Melhorias futuras (se eu continuasse)

- Colocar autenticação (login)
- Dashboard com gráficos de gastos por categoria
- Filtros de transação por período e pessoa
- Publicar em nuvem (frontend + API)
