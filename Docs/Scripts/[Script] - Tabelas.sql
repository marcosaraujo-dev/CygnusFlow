use progestao
GO

-- Tabelas auxiliares
CREATE TABLE Equipe (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
	Descricao NVARCHAR(200) NULL DEFAULT '',
	Ativo  bit Default 1
);

CREATE TABLE StatusUsuario (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL,
	Descricao NVARCHAR(200) NULL DEFAULT '',
	Ativo  bit Default 1-- Ativo, Inativo, Bloqueado
);

CREATE TABLE ModuloSistema (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
	Descricao NVARCHAR(200) NULL DEFAULT '',
	Ativo  bit Default 1
);

CREATE TABLE Criticidade (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL, -- Alta, Média, Baixa
	Descricao NVARCHAR(200) NULL DEFAULT '',
	Cor NVARCHAR(7),
	Nivel Int NOT NULL, -- 1 = Baixa, 2 = Média, 3 = Alta, 4 = Crítica
	Ativo  bit Default 1
);

CREATE TABLE StatusProjeto (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL, -- Planejado, Em Andamento, Concluído, Cancelado
	Descricao NVARCHAR(200) NULL DEFAULT '',
	Ativo  bit Default 1
);

CREATE TABLE TipoAtividade (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
	Descricao NVARCHAR(200) NULL DEFAULT '',
	Cor NVARCHAR(7),
	Ativo  bit Default 1
);

CREATE TABLE TipoUsuario (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL, -- Admin, PO, Dev, Viewer
	Descricao NVARCHAR(200) NULL DEFAULT '',
	Ativo  bit Default 1
);

-- Tabela de usuários
CREATE TABLE Usuario (
    Id INT IDENTITY PRIMARY KEY,
    Nome NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    SenhaHash NVARCHAR(255) NOT NULL,
    EquipeId INT,
    TipoUsuarioId INT NOT NULL,
    StatusUsuarioId INT NOT NULL,
    BloqueadoPorRedefinicao BIT DEFAULT 0,
    DataCadastro DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Usuario_Equipe FOREIGN KEY (EquipeId) REFERENCES Equipe(Id),
    CONSTRAINT FK_Usuario_TipoUsuario FOREIGN KEY (TipoUsuarioId) REFERENCES TipoUsuario(Id),
    CONSTRAINT FK_Usuario_StatusUsuario FOREIGN KEY (StatusUsuarioId) REFERENCES StatusUsuario(Id)
);

-- Tabela de redefinição de senha
CREATE TABLE UsuarioRedefinicaoSenha (
    Id INT IDENTITY PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Codigo NVARCHAR(10) NOT NULL,
    GeradoPorAdmin BIT DEFAULT 0,
    Utilizado BIT DEFAULT 0,
    ExpiraEm DATETIME NOT NULL,
    DataGeracao DATETIME DEFAULT GETDATE(),
    DataUtilizacao DATETIME NULL,

    CONSTRAINT FK_UsuarioRedefinicaoSenha_Usuario FOREIGN KEY (UsuarioId)
        REFERENCES Usuario(Id)
);

-- Tabela de projetos
CREATE TABLE Projeto (
    Id INT IDENTITY PRIMARY KEY,
    Codigo NVARCHAR(20) NOT NULL UNIQUE, -- Ex: PRJ-00001
    Nome NVARCHAR(200) NOT NULL,
    ModuloId INT NOT NULL,
    CriticidadeId INT NOT NULL,
    DataInicioPO DATE NOT NULL,
    DataFimPO DATE NOT NULL,
    EstimativaHoras INT NOT NULL,
    StatusProjetoId INT NOT NULL DEFAULT 1,
    DataCadastro DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Projeto_Modulo FOREIGN KEY (ModuloId) REFERENCES ModuloSistema(Id),
    CONSTRAINT FK_Projeto_Criticidade FOREIGN KEY (CriticidadeId) REFERENCES Criticidade(Id),
    CONSTRAINT FK_Projeto_Status FOREIGN KEY (StatusProjetoId) REFERENCES StatusProjeto(Id)
);

-- Tabela de comentários por projeto
CREATE TABLE ProjetoComentario (
    Id INT IDENTITY PRIMARY KEY,
    ProjetoId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(MAX) NOT NULL,
    DataComentario DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_ProjetoComentario_Projeto FOREIGN KEY (ProjetoId) REFERENCES Projeto(Id),
    CONSTRAINT FK_ProjetoComentario_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-- Tabela de atividades
CREATE TABLE Atividade (
    Id INT IDENTITY PRIMARY KEY,
    Codigo NVARCHAR(20) NOT NULL UNIQUE, -- Ex: ATV-00001
    Nome NVARCHAR(200) NOT NULL,
    ProjetoId INT NOT NULL,
    ResponsavelId INT NOT NULL,
    TipoAtividadeId INT NOT NULL,
  --  DataInicioEstimada DATE NOT NULL,
  --  DataFimEstimada DATE NOT NULL,

    DataInicioPlanejada DATE NULL,
    DataFimPlanejada DATE NULL,
    DataInicioReal DATE NULL,
    DataFimReal DATE NULL,

    StatusProjetoId INT NOT NULL DEFAULT 1,
    Observacoes NVARCHAR(MAX),
    Impedimentos NVARCHAR(MAX),
    DataCadastro DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Atividade_Projeto FOREIGN KEY (ProjetoId) REFERENCES Projeto(Id),
    CONSTRAINT FK_Atividade_Usuario FOREIGN KEY (ResponsavelId) REFERENCES Usuario(Id),
    CONSTRAINT FK_Atividade_Tipo FOREIGN KEY (TipoAtividadeId) REFERENCES TipoAtividade(Id),
    CONSTRAINT FK_Atividade_Status FOREIGN KEY (StatusProjetoId) REFERENCES StatusProjeto(Id)
);

-- Tabela de comentários por atividade
CREATE TABLE AtividadeComentario (
    Id INT IDENTITY PRIMARY KEY,
    AtividadeId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(MAX) NOT NULL,
    DataComentario DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_AtividadeComentario_Atividade FOREIGN KEY (AtividadeId) REFERENCES Atividade(Id),
    CONSTRAINT FK_AtividadeComentario_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);
