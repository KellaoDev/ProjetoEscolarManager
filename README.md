# 📘 **Projeto De Estágio — EscolarManager**

Sistema desenvolvido como parte do meu estágio, utilizando **C#**, **ASP.NET MVC** e uma arquitetura em camadas para aplicar boas práticas de **Clean Architecture**, **SOLID** e um **DDD simplificado**.

O objetivo é fornecer um gerenciamento simples de dados escolares, incluindo alunos, cidades e relatórios.

---

## 🏛 **Arquitetura do Projeto**

O sistema segue uma arquitetura em **3 camadas**, separando com clareza responsabilidades:

### **🔹 EM.Domain (Domínio)**
- Contém entidades centrais do negócio (Aluno, Cidade)
- Regras de negócio e validações específicas (ex.: CPF, Data de Nascimento)
- Enums e interfaces do domínio

### **🔹 EM.Repository (Infraestrutura de Dados)**
- Implementações de repositórios
- Interfaces de repositórios
- Contexto de banco e extensões
- Depende do domínio, mas o domínio não depende dele

### **🔹 EM.Web (Aplicação / Apresentação)**
- Controllers
- Views (Razor)
- DTOs
- Models
- Serviços de aplicação (ex.: geração de relatórios em PDF)
- Responsável pelo fluxo apresentação → domínio → dados
---

## ⚙️ **Tecnologias Utilizadas**

### **Back-end**
- C# 12  
- .NET 8 / ASP.NET MVC  
- Entity Framework Core  
- iTextSharp (Geração de PDF)  
- LINQ  
- Injeção de Dependência nativa do .NET  

### **Front-end**
- HTML
- Bootstrap
- Razor Views  

### **Padrões e Princípios**
- SOLID  
- DDD Simplificado  
- Repository Pattern  
- Clean Architecture  

---

## 🚀 **Como Executar o Projeto**

### 🔧 **Pré-requisitos**
- .NET SDK 8.0 ou superior  
- Firebird 
- Visual Studio 2022

---

### ▶️ **Passo a passo**

1. Clone o repositório:
   ```bash
   git clone https://github.com/KellaoDev/ProjetoEscolarManager.git

2. Entre na pasta do projeto:
   ```bash
   cd ProjetoEscolarManager

3. Restaure pacotes e compile
   ```bash
   dotnet restore
   dotnet build

4. Configure a conexão com o banco (ex.: DBHelper.cs):
    ```bash
     @"Server=localhost; Port=3055; 
                     Database=C:\\WorkKelio\\ProjetoEscolarManager
                     \\EM.Repository\\Database\\BANCO.fdb;
                     User=SYSDBA;
                     Password=masterkey;";

5. Execute a aplicação (Visual Studio: abrir solução e rodar; CLI):
   ```bash
    dotnet run --project EM.Web

