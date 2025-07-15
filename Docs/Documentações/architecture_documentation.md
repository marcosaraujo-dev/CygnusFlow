# 🏗️ Arquitetura CygnusFlow - Sistema de Gestão de Projetos

## 📋 Visão Geral

O sistema CygnusFlow foi projetado seguindo os princípios da **Clean Architecture**, garantindo:
- **Separação de responsabilidades**
- **Independência de frameworks**
- **Testabilidade**
- **Flexibilidade para mudanças**

## 📁 Estrutura de Pastas

```
CygnusFlow/
├── 📁 src/
│   ├── 📁 CygnusFlow.Domain/           # 🎯 DOMAIN LAYER
│   │   ├── 📁 Entities/
│   │   ├── 📁 ValueObjects/
│   │   ├── 📁 Services/
│   │   ├── 📁 Repositories/
│   │   └── 📁 Shared/
│   │       ├── 📁 Notifications/
│   │       ├── 📁 Results/
│   │       └── 📁 Constants/
│   │
│   ├── 📁 CygnusFlow.Application/      # 🔧 APPLICATION LAYER
│   │   ├── 📁 UseCases/
│   │   ├── 📁 DTOs/
│   │   ├── 📁 Services/
│   │   ├── 📁 Validation/
│   │   └── 📁 Interfaces/
│   │       ├── 📁 Infrastructure/
│   │       └── 📁 External/
│   │
│   ├── 📁 CygnusFlow.Infrastructure/   # 🏗️ INFRASTRUCTURE LAYER
│   │   ├── 📁 Data/
│   │   │   ├── 📁 Context/
│   │   │   ├── 📁 Repositories/
│   │   │   └── 📁 Migrations/
│   │   ├── 📁 Logging/
│   │   ├── 📁 Metrics/
│   │   ├── 📁 Serialization/
│   │   ├── 📁 Audit/
│   │   ├── 📁 ExternalServices/
│   │   └── 📁 Configuration/
│   │
│   ├── 📁 CygnusFlow.API/              # 🌐 API LAYER
│   │   ├── 📁 Controllers/
│   │   ├── 📁 Middleware/
│   │   ├── 📁 Filters/
│   │   ├── 📁 Extensions/
│   │   └── 📁 Configuration/
│   │
│   └── 📁 CygnusFlow.WinForms/         # 🖥️ PRESENTATION LAYER
│       ├── 📁 Forms/
│       ├── 📁 Services/
│       ├── 📁 Controls/
│       └── 📁 Helpers/
│
├── 📁 tests/
└── 📁 docs/
```

## 🎯 Camadas da Arquitetura

### 1. **Domain Layer** (Núcleo do Sistema)
**Responsabilidade:** Regras de negócio puras, sem dependências externas

#### Shared (Classes Bases Padrões)
- **Result**: Representa a estrutura base para retorno do processo (Padronizando os retornos).
- **ResultList**: Representa uma lista de retornos do processo.
- **NotificationResult**: Representa a classe base do padrão Result, centralizando as notificações ocorridas durante o processo.
- **Error**: Representa as notificações de erros
- **Warning**: Representa as notificações de alertas.
- **Success**: Representa as notificações de sucesso.
<!--  -->
#### Entities (Entidades)
- **Usuario**: Representa usuários do sistema com autenticação e permissões
- **Projeto**: Entidade principal para gestão de projetos
- **Atividade**: Tarefas vinculadas aos projetos
- **Entidades Auxiliares**: Equipe, StatusUsuario, ModuloSistema, etc.

#### Value Objects
- **CodigoProjeto**: Garante formato PRJ-00001 e unicidade
- **CodigoAtividade**: Garante formato ATV-00001 e unicidade
- **PeriodoProjeto**: Encapsula lógica de validação de datas

#### Domain Services
- **ProjetoService**: Validações complexas de projetos
- **UsuarioService**: Lógica de autenticação e permissões

#### Repository Interfaces
- Contratos para acesso aos dados
- Seguem o princípio da inversão de dependência

### 2. **Application Layer** (Casos de Uso)
**Responsabilidade:** Coordena as operações do sistema

#### Use Cases
Implementam os casos de uso específicos do sistema:
- **CriarProjetoUseCase**: Coordena criação de projetos
- **LoginUseCase**: Gerencia autenticação
- **RedefinirSenhaUseCase**: Processo de redefinição de senha
- **GerarGanttUseCase**: Gera relatórios Gantt

#### DTOs (Data Transfer Objects)
- **Request DTOs**: Dados vindos da interface
- **Response DTOs**: Dados retornados para a interface
- **List DTOs**: Dados para listagens

#### Services
- **AuthService**: Gerenciamento de tokens JWT
- **EmailService**: Envio de emails
- **RelatorioService**: Geração de relatórios

### 3. **Infrastructure Layer** (Infraestrutura)
**Responsabilidade:** Implementação de detalhes técnicos

#### Data Access
- **AppDbContext**: Contexto do Entity Framework
- **Repositories**: Implementações concretas dos repositórios
- **Migrations**: Evolução do schema do banco

#### External Services
- **EmailProvider**: Integração com provedor de email
- **FileService**: Manipulação de arquivos
- **JwtService**: Geração e validação de tokens

### 4. **API Layer** (Interface Web)
**Responsabilidade:** Exposição de endpoints REST

#### Controllers
- **AuthController**: Endpoints de autenticação
- **UsuarioController**: CRUD de usuários
- **ProjetoController**: CRUD de projetos
- **AtividadeController**: CRUD de atividades
- **RelatorioController**: Geração de relatórios

#### Middleware
- **AuthMiddleware**: Validação de tokens
- **ExceptionMiddleware**: Tratamento de erros

#### Filters
- **AuthorizeFilter**: Controle de permissões
- **ValidationFilter**: Validação de dados

### 5. **WinForms Layer** (Interface Desktop)
**Responsabilidade:** Interface com o usuário

#### Forms
- **LoginForm**: Tela de login
- **MainForm**: Tela principal com menu
- **ProjetoForm**: Cadastro de projetos
- **AtividadeForm**: Cadastro de atividades
- **RelatorioForm**: Visualização de relatórios
- **UsuarioForm**: Gerenciamento de usuários

#### Services
- **ApiService**: Comunicação com a API
- **FormService**: Utilitários para forms

## 🔄 Fluxos Principais

### 1. **Fluxo de Autenticação**
```
WinForms → API → Application → Domain → Infrastructure → JWT
```
- Usuário informa credenciais
- Sistema valida no banco
- Gera token JWT
- Armazena token no cliente

### 2. **Fluxo de Criação de Projeto**
```
Form → Validação → API → UseCase → Domain → Repository → Database
```
- Validação client-side
- Validação server-side
- Geração de código único
- Persistência no banco

### 3. **Fluxo de Relatórios**
```
Form → Filtros → API → UseCase → Repository → Service → Export
```
- Aplicação de filtros
- Busca dados
- Processamento
- Geração de arquivo

## 🔐 Segurança

### Autenticação
- **JWT Tokens** com expiração
- **Hashing** seguro de senhas (bcrypt)
- **Redefinição** de senha com código temporário

### Autorização
- **Filtros** de autorização por endpoint
- **Níveis** de usuário (Admin, PO, Dev, Viewer)
- **Validação** de permissões no domain

### Proteção
- **SQL Injection**: Entity Framework + Parametrização
- **XSS**: Validação de entrada
- **HTTPS**: Comunicação segura

## 📊 Padrões Utilizados

### Design Patterns
- **Repository Pattern**: Abstração de acesso aos dados
- **Unit of Work**: Gerenciamento de transações
- **Dependency Injection**: Inversão de controle
- **CQRS**: Separação de commands e queries (futuro)

### Princípios SOLID
- **SRP**: Cada classe tem uma responsabilidade
- **OCP**: Aberto para extensão, fechado para modificação
- **LSP**: Substituição de Liskov
- **ISP**: Segregação de interfaces
- **DIP**: Inversão de dependência

## 🧪 Estratégia de Testes

### Testes por Camada
- **Domain**: Testes unitários das regras de negócio
- **Application**: Testes de casos de uso
- **Infrastructure**: Testes de integração
- **API**: Testes de endpoints
- **WinForms**: Testes de interface (manual/automatizado)

### Tipos de Teste
- **Unit Tests**: Lógica isolada
- **Integration Tests**: Interação entre camadas
- **End-to-End Tests**: Fluxo completo

## 🚀 Vantagens desta Arquitetura

### Manutenibilidade
- **Separação clara** de responsabilidades
- **Baixo acoplamento** entre camadas
- **Alta coesão** dentro das camadas

### Testabilidade
- **Injeção de dependência** facilita mocks
- **Interfaces** permitem substituição
- **Lógica** isolada das dependências

### Flexibilidade
- **Troca de tecnologias** sem impacto no negócio
- **Evolução** independente das camadas
- **Reutilização** de componentes

### Escalabilidade
- **Adição** de novas funcionalidades
- **Modificação** de comportamentos
- **Expansão** para outras interfaces (Web, Mobile)

## 📈 Roadmap de Evolução

### Fase 1: MVP
- ✅ Estrutura básica
- ✅ CRUD principal
- ✅ Autenticação simples

### Fase 2: Funcionalidades Avançadas
- 🔄 Relatórios Gantt
- 🔄 Dashboard
- 🔄 Notificações

### Fase 3: Otimizações
- 📋 Cache
- 📋 CQRS
- 📋 Event Sourcing

### Fase 4: Expansão
- 📋 Interface Web (React)
- 📋 Mobile App
- 📋 Integrações

## 🛠️ Tecnologias Chave

### Backend
- **.NET 9**: Framework principal
- **Entity Framework Core**: ORM
- **JWT**: Autenticação
- **AutoMapper**: Mapeamento de objetos
- **FluentValidation**: Validação

### Frontend
- **Windows Forms**: Interface desktop
- **React** (Fase 4): Interface web

### Banco de Dados
- **SQL Server**: Banco principal
- **Migrations**: Evolução do schema

### Ferramentas
- **Swagger**: Documentação da API
- **xUnit**: Framework de testes
- **Moq**: Mocking para testes