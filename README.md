# 📖 BookFlow — Backend API

Bem-vindo ao repositório do backend do **BookFlow**, uma API REST desenvolvida em **.NET 10** que gerencia uma plataforma completa de empréstimo e venda de livros digitais.

Esta aplicação implementa fluxos financeiros, autenticação segura baseada em roles e controle de acesso estrito. O projeto nasceu de uma ideia de CRUD de empréstimos, mas evoluiu para uma plataforma robusta de marketplace para dois perfis independentes: **Editora** e **Cliente**.

## 🚀 Stack Tecnológica

- **.NET 10** — Web API
- **Entity Framework Core** — ORM moderno com abordagem Code-First.
- **SQL Server** — Banco de dados relacional (via pacotes Microsoft.EntityFrameworkCore.SqlServer).
- **JWT (JSON Web Tokens)** — Autenticação e autorização por claims.
- **BCrypt.Net-Next** — Hash e validação segura de senhas.
- **Rate Limiting** — Proteção nativa no middleware do ASP.NET.

## 🏗️ Arquitetura e Padrões

O projeto segue princípios **SOLID** e foi estruturado com o **Repository Pattern** em uma Arquitetura em Camadas (Clean Architecture) focada no domínio:

- **Controllers:** Camada de apresentação da API, recebem requisições HTTP e extraem dados de segurança do Token.
- **Services:** Concentram toda a regra de negócio, validando estoques, orquestrando fluxos e lidando com exceções.
- **Repositories:** Abstraem o acesso a dados via Entity Framework Core.
- **DTOs:** Objetos independentes para separar os modelos de banco de dados das requisições da web, evitando vazamento de dados (over-posting).

## 🛡️ Destaques de Segurança e Regras de Negócio

1. **Identity & Role-Based Access Control:** 
   Clientes e Editoras possuem papéis distintos. A extração de identificadores sensíveis (como `idEditora`) é feita **sempre** direto do Token JWT (usando `User.FindFirst`), impossibilitando falhas de IDOR caso o frontend envie um payload adulterado.

2. **Fluxo Financeiro Fechado:**
   - Cada compra desencadeia *duas* movimentações cruzadas em carteiras de diferentes donos: **Débito** no Cliente e **Crédito** na Editora.
   - O banco de dados possui uma `Check Constraint` (`CK_Carteira_Dono`) garantindo que nenhuma carteira pertença, acidentalmente, a dois atores.

3. **Rate Limiting (Prevenção de Brute Force):**
   Endpoints de login (`/login`, `/login/editora`) utilizam o _Fixed Window Limiter_ nativo do .NET para barrar picos de tentativas de senhas incorretas, mantendo as contas financeiras seguras.

4. **Paginação Segura:**
   A API conta com um `PaginacaoRequestDto` interceptando pesquisas (como as de Livros) com um `Range` máximo de **50 registros**, protegendo o banco contra consultas exaustivas (DDoS no nível da base).

## 📊 Endpoints Principais

A API é segmentada nos seguintes fluxos lógicos:

- **Auth:** Login de Cliente e Editora (`POST /api/cliente/login`, `POST /api/editora/login`)
- **Carteira:** Saques, depósitos e extratos independentes. Multas por atraso de empréstimo (R$ 3,00/dia) são debitadas de modo automático.
- **Livro:** Cadastro/Atualização para editoras; Busca com filtro e paginação aberta para clientes.
- **Compra & Empréstimo:** Criação de transações que validam estoque, limites da carteira e geram históricos correspondentes.
- **Métricas:** Painel exclusivo para Editoras exibindo receita, total vendido e top performers.

## 💻 Como Rodar o Projeto (Local)

1. Clone o repositório.
2. Certifique-se de ter o **.NET 10 SDK** instalado em sua máquina.
3. Configure a _Connection String_ do SQL Server em seu `appsettings.json` ou `appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=SEU_SERVIDOR;Database=BookFlowDb;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```
4. Aplique as migrações no banco de dados. No **Package Manager Console** do Visual Studio, rode:
   ```powershell
   Update-Database
   ```
   *Ou via CLI:* `dotnet ef database update`
5. Inicie a aplicação (via VS ou `dotnet run`). A API abrirá na porta configurada, expondo o Swagger para testes imediatos.

---

_Desenvolvido com foco na integridade das regras de negócio, proteção contra manipulação de dados e arquitetura sustentável._
