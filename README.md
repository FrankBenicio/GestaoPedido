
# 🛠️ Desafio Técnico – API de Gestão de Pedidos

Projeto desenvolvido com foco em boas práticas de arquitetura, testes automatizados e modularidade, simulando uma aplicação real de controle de pedidos com clientes, produtos e autenticação via JWT.

---

## 📐 Metodologias & Arquitetura

- **Clean Architecture**
- **TDD (Test-Driven Development)**
- **Design Modular**
- **Use Cases Orientados a Domínio**

---

## ⚙️ Tecnologias & Ferramentas

- [.NET 9.0](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger (Swashbuckle)
- xUnit
- Moq
- FluentAssertions
- Stryker.NET (Testes de mutação)
- RestSharp
- Git
- Visual Studio / VS Code

---

## ✅ Cobertura de Testes

- Testes **unitários**
- Testes de **integração**
- **Mocks** com Moq
- **Testes de mutação** com Stryker
- **Cobertura de código** automatizada

---

## 📡 API REST

A API disponibiliza endpoints com os verbos:

- `POST` – Criar recursos (ex: clientes, produtos, pedidos)
- `GET` – Listar ou buscar por ID
- `PUT` – Atualizar recursos
- `DELETE` – Exclusão física e lógica

Autenticação protegida com **JWT** (exceto login e criação de usuários).

---

## ▶️ Como Executar o Projeto

### 🔧 Pré-requisitos

- .NET SDK 9.0 ou superior
- SQL Server LocalDB (ou instância configurada)
- Visual Studio ou VS Code com extensão C#

### 🚀 Passo a passo

1. **Clone o repositório**  
   ```bash
   git clone https://github.com/seu-usuario/seu-repo.git
   cd seu-repo
   ```

2. **Crie o banco de dados**  
   Execute o comando:
   ```bash
   dotnet ef database update
   ```
   > Ou utilize o script SQL disponível na pasta `/Scripts`.

3. **Rode a aplicação**
   ```bash
   dotnet run --project GestaoPedido.API
   ```

4. **Acesse a interface Swagger**
   ```
   https://localhost:7181/swagger
   ```

---

## 🖼️ Swagger UI

> Interface interativa para explorar os endpoints da API.

<img src="https://i.imgur.com/q9XWZkH.png" alt="Swagger UI da API" width="100%">

---

## 🤝 Contribuição

Este projeto foi desenvolvido como entrega de desafio técnico, com foco em qualidade, clareza e aderência às melhores práticas modernas de desenvolvimento.
