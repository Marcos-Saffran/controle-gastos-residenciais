# Frontend - Controle de Gastos

Esse é o frontend do projeto de controle de gastos residenciais.

Foi feito com React + TypeScript e consome os endpoints da API.

## O que tem na interface

- telas para listar e cadastrar pessoas
- telas para listar e cadastrar categorias
- telas para listar e cadastrar transações

## Tecnologias

- React
- TypeScript
- Axios
- React Router

## Pré-requisitos

- Node.js 18+
- npm

## Como executar

No terminal dentro desta pasta (`frontend/gastos-frontend`):

```bash
npm install
npm start
```

Aplicação abre em:
- http://localhost:3000

## Comunicação com backend

A URL base da API está em `src/api/api.ts`:

```ts
baseURL: "http://localhost:5000/api"
```

Se a API rodar em outra porta, precisa ajustar esse arquivo.

## Scripts disponíveis

- `npm start`: roda em desenvolvimento
- `npm run build`: gera build de produção

## Observação

Para funcionar completo, o backend precisa estar rodando junto.
