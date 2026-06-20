# Cadastro de Pessoas
 
Aplicação desktop em WPF para cadastro, edição, exclusão e pesquisa de pessoas, consumindo uma API REST construída em ASP.NET Core com Entity Framework Core e SQLite.
 
A solução é composta por dois projetos:
 
- **Cadastro.Api** — Web API em ASP.NET Core responsável pela persistência dos dados.
- **Cadastro.Wpf** — Aplicação desktop em WPF (cliente) que consome a API.

## Tecnologias
 
| Camada | Stack |
|---|---|
| Backend (API) | ASP.NET Core 10 (`net10.0`), Entity Framework Core 10, EF Core Sqlite, Swashbuckle (Swagger) |
| Frontend (Desktop) | WPF (`net10.0-windows`), C# (Nullable + Implicit Usings habilitados) |
| Banco de dados | SQLite (`cadastro.db`) |
| Comunicação | HTTP REST (`HttpClient` + `System.Net.Http.Json`) |
 
## Arquitetura
 
O `Cadastro.Api` segue uma separação em camadas:
 
- **Controllers** — expõem os endpoints HTTP (`PessoaController`)
- **Services** — regras de negócio (`IPessoaService` / `PessoaService`)
- **Repositories** — acesso a dados (`IPessoaRepository` / `PessoaRepository`)
- **Data** — `AppDbContext` (EF Core)
- **DTOs / Models** — objetos de requisição/resposta (`PessoaPostRequest`, `PessoaPutRequest`, etc.)
- **Migrations** — histórico de migrações do EF Core
O `Cadastro.Wpf` segue o padrão **MVVM**:
 
- **Views** — telas XAML (`MainWindow`, `PessoaDialog`)
- **ViewModels** — lógica de apresentação e binding (`MainViewModel`)
- **Models** — entidades do domínio no cliente (`Pessoa`)
- **Services** — comunicação com a API (`ApiService`)
- **Helpers** — utilitários (ex: `TextHelper` para formatação/limpeza de texto)


## Configuração
 
### API
 
A connection string do SQLite é lida de `appsettings.json`, na chave `ConnectionStrings:DefaultConnection`. Por padrão, o banco utilizado é o arquivo `cadastro.db` na raiz do projeto `Cadastro.Api`.
 
As migrations do EF Core já existem no projeto. Para aplicá-las (caso o banco não exista ou esteja desatualizado):
 
```bash
cd Cadastro.Api
dotnet ef database update
```
 
### WPF (cliente)
 
O endereço base da API está fixo em `ApiService.cs`:
 
```csharp
_httpClient.BaseAddress = new Uri("http://localhost:5042/api/");
```
 
## Como executar
 
1. **Suba a API primeiro:**
```bash
cd Cadastro.Api
dotnet run
```
 
2. **Em seguida, execute o cliente WPF:**
```bash
cd Cadastro.Wpf
dotnet run
```
 
## Endpoints da API
 
Base: `/api/Pessoa`
 
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/Pessoa` | Lista todas as pessoas |
| `GET` | `/api/Pessoa/{id}` | Busca uma pessoa por id |
| `POST` | `/api/Pessoa` | Cria uma nova pessoa |
| `PUT` | `/api/Pessoa/{id}` | Atualiza uma pessoa existente |
| `DELETE` | `/api/Pessoa/{id}` | Remove uma pessoa |
 
## Funcionalidades
 
- **Listagem** de pessoas cadastradas, com nome, sobrenome e telefone formatado automaticamente (ex: `(41) 99999-9999`).
<img width="879" height="586" alt="image" src="https://github.com/user-attachments/assets/6608bb14-a14d-42c8-ba17-9810a6978dae" />

- **Cadastro** de nova pessoa via diálogo (`PessoaDialog`).
<img width="879" height="585" alt="image" src="https://github.com/user-attachments/assets/2f7664ed-2772-4522-8030-fa7505010c6a" />

- **Edição** de pessoa existente via menu de contexto (⋮) em cada item da lista.
- **Exclusão** de pessoa, com confirmação prévia (`MessageBox`).
<img width="992" height="626" alt="image" src="https://github.com/user-attachments/assets/d0a742fa-be5a-4611-862b-0a9d80568a55" />

- **Pesquisa** em tempo real por nome, sobrenome, nome completo ou telefone (ignorando formatação).
<img width="883" height="588" alt="image" src="https://github.com/user-attachments/assets/1bdeb593-7a20-4a82-8166-82dd1cda6ec0" />

- **Loading state**: indicador visual enquanto os dados são carregados da API.
<img width="883" height="585" alt="image" src="https://github.com/user-attachments/assets/dff5e824-71f9-4499-adab-1d460caa835a" />

- **Empty state**: mensagem amigável quando não há pessoas cadastradas.
<img width="884" height="587" alt="image" src="https://github.com/user-attachments/assets/f529124a-a157-4acd-89ec-70484689434a" />

- **Tratamento de erros**: falhas de comunicação com a API são exibidas em `MessageBox` ao usuário.

## Futuro hipotético da aplicação
 
O projeto atual prioriza simplicidade (CRUD direto, sem autenticação, banco SQLite local). Em um cenário de evolução para produção, os focos de **endurecimento (hardening)** da API seriam: Validação e tratamento de erros, melhoria da infraestrutura de dados, autenticação e autorização.
 
