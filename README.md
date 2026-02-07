# Ecommerce Orders API

API de pedidos com arquitetura limpa, processamento assíncrono via mensageria e worker de background.

## Visão Geral

Este projeto contém:

- `Ecommerce.Api`: API REST para criar, consultar, atualizar e cancelar pedidos.
- `Ecommerce.Worker`: serviço worker que consome eventos e processa pedidos em background.
- `Ecommerce.Application`, `Ecommerce.Domain`, `Ecommerce.Infrastructure`: camadas de negócio e persistência.
- `Ecommerce.Tests`: testes unitários da camada de dom�nio.

Fluxo principal:

1. A API recebe um novo pedido (`POST /order`).
2. O pedido é persistido no SQL Server e um evento é publicado no RabbitMQ.
3. O Worker consome o evento e agenda/processa o pedido com Hangfire.
4. O status do pedido é atualizado para `Processed`.

## Stack

- .NET 10 (`net10.0`)
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- MassTransit + RabbitMQ
- Hangfire (armazenamento no SQL Server)
- Serilog + MongoDB (logs)
- xUnit (testes)

## Pré-requisitos

- .NET SDK 10 instalado
- Docker + Docker Compose

Opcional (para executar migrations manualmente):

- `dotnet-ef`

```bash
dotnet tool install --global dotnet-ef
```

## Subindo com Docker Compose (recomendado)

Na raiz do repositório:

```bash
docker compose up --build
```

Servicos disponíveis:

- Swagger: `http://localhost:5096/swagger`
- Hangfire Dashboard: `http://localhost:5096/hangfire`
- RabbitMQ Management: `http://localhost:15672`

## Executando localmente (API + Worker)

1. Suba apenas as dependências de infraestrutura:

```bash
docker compose up -d sqlserver rabbitmq mongo
```

2. Aplique as migrations no banco:

```bash
dotnet ef database update --project Ecommerce.Infrastructure --startup-project Ecommerce.Api
```

3. Rode a API:

```bash
dotnet run --project Ecommerce.Api
```

4. Em outro terminal, rode o Worker:

```bash
dotnet run --project Ecommerce.Worker
```

## Configuração

As configurações padrão estão em:

- `Ecommerce.Api/appsettings.json`
- `Ecommerce.Worker/appsettings.json`

Principais chaves:

- `ConnectionStrings:SqlConnection`
- `RabbitMq:Host`, `RabbitMq:VirtualHost`, `RabbitMq:Username`, `RabbitMq:Password`
- `MongoDb:ConnectionString`, `MongoDb:DatabaseName` (API)

## Endpoints Principais

- `POST /order` cria pedido
- `GET /orders?page=0&size=4&status=Received` lista paginada
- `GET /order/{id}` consulta por id
- `PUT /order/{id}` atualiza pedido
- `DELETE /order/{id}` cancela pedido

## Rodando os testes

```bash
dotnet test Ecommerce.Tests/Ecommerce.Tests.csproj
```

## Estrutura de pastas

- `Ecommerce.Api/`
- `Ecommerce.Application/`
- `Ecommerce.Domain/`
- `Ecommerce.Infrastructure/`
- `Ecommerce.Worker/`
- `Ecommerce.Tests/`
