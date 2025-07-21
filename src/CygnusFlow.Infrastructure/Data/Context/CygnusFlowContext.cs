using CygnusFlow.Infrastructure.Data.Models;

using StatusUsuario = CygnusFlow.Domain.Enums.StatusUsuario;
using TipoUsuario = CygnusFlow.Domain.Enums.TipoUsuario;
using Microsoft.EntityFrameworkCore;
using System;

namespace CygnusFlow.Infrastructure.Data.Context
{
    public class CygnusFlowContext : DbContext
    {
        public CygnusFlowContext(DbContextOptions<CygnusFlowContext> options) : base(options) { }

        // DbSets usando os Models do EF
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<ProjetoModel> Projetos { get; set; }
        public DbSet<AtividadeModel> Atividades { get; set; }
        public DbSet<EquipeModel> Equipes { get; set; }
        public DbSet<ModuloSistemaModel> ModulosSistema { get; set; }
        public DbSet<ProjetoComentarioModel> ProjetoComentarios { get; set; }
        public DbSet<AtividadeComentarioModel> AtividadeComentarios { get; set; }
        public DbSet<CriticidadeModel> Criticidades { get; set; }
        public DbSet<StatusProjetoModel> StatusProjetos { get; set; }
        public DbSet<StatusUsuarioModel> StatusUsuarios { get; set; }
        public DbSet<TipoUsuarioModel> TiposUsuario { get; set; }
        public DbSet<TipoAtividadeModel> TiposAtividade { get; set; }
        public DbSet<UsuarioRedefinicaoSenhaModel> UsuarioRedefinicaoSenhas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração Usuario
            modelBuilder.Entity<UsuarioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.SenhaHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.BloqueadoPorRedefinicao).HasDefaultValue(false);
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");

                // Relacionamentos
                entity.HasOne(u => u.Equipe)
                    .WithMany(e => e.Usuarios)
                    .HasForeignKey(u => u.EquipeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(u => u.TipoUsuario)
                    .WithMany(t => t.Usuarios)
                    .HasForeignKey(u => u.TipoUsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.StatusUsuario)
                    .WithMany(s => s.Usuarios)
                    .HasForeignKey(u => u.StatusUsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração Projeto
            modelBuilder.Entity<ProjetoModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DataInicioPO).IsRequired();
                entity.Property(e => e.DataFimPO).IsRequired();
                entity.Property(e => e.EstimativaHoras).IsRequired();
                entity.Property(e => e.StatusProjetoId).HasDefaultValue(1);
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");

                // Relacionamentos - CORRIGIDOS
                entity.HasOne(p => p.Modulo)
                    .WithMany(m => m.Projetos)
                    .HasForeignKey(p => p.ModuloId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Criticidade)
                    .WithMany(c => c.Projetos)
                    .HasForeignKey(p => p.CriticidadeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.StatusProjeto)
                    .WithMany(s => s.Projetos)
                    .HasForeignKey(p => p.StatusProjetoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração Atividade
            modelBuilder.Entity<AtividadeModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StatusProjetoId).HasDefaultValue(1);
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");

                // Relacionamentos - CORRIGIDOS
                entity.HasOne(a => a.Projeto)
                    .WithMany(p => p.Atividades)
                    .HasForeignKey(a => a.ProjetoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Responsavel)
                    .WithMany(u => u.AtividadesResponsavel)
                    .HasForeignKey(a => a.ResponsavelId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.TipoAtividade)
                    .WithMany(t => t.Atividades)
                    .HasForeignKey(a => a.TipoAtividadeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.StatusProjeto)
                    .WithMany(s => s.Atividades)
                    .HasForeignKey(a => a.StatusProjetoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração ProjetoComentario
            modelBuilder.Entity<ProjetoComentarioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Comentario).IsRequired();
                entity.Property(e => e.DataComentario).HasDefaultValueSql("GETDATE()");

                entity.HasOne(pc => pc.Projeto)
                    .WithMany(p => p.Comentarios)
                    .HasForeignKey(pc => pc.ProjetoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pc => pc.Usuario)
                    .WithMany(u => u.ProjetoComentarios)
                    .HasForeignKey(pc => pc.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração AtividadeComentario
            modelBuilder.Entity<AtividadeComentarioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Comentario).IsRequired();
                entity.Property(e => e.DataComentario).HasDefaultValueSql("GETDATE()");

                entity.HasOne(ac => ac.Atividade)
                    .WithMany(a => a.Comentarios)
                    .HasForeignKey(ac => ac.AtividadeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ac => ac.Usuario)
                    .WithMany(u => u.AtividadeComentarios)
                    .HasForeignKey(ac => ac.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração UsuarioRedefinicaoSenha
            modelBuilder.Entity<UsuarioRedefinicaoSenhaModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.GeradoPorAdmin).HasDefaultValue(false);
                entity.Property(e => e.Utilizado).HasDefaultValue(false);
                entity.Property(e => e.ExpiraEm).IsRequired();
                entity.Property(e => e.DataGeracao).HasDefaultValueSql("GETDATE()");

                entity.HasOne(ur => ur.Usuario)
                    .WithMany()
                    .HasForeignKey(ur => ur.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurações entidades auxiliares
            modelBuilder.Entity<EquipeModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(200).HasDefaultValue("");
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            modelBuilder.Entity<ModuloSistemaModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(200).HasDefaultValue("");
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            modelBuilder.Entity<CriticidadeModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descricao).HasMaxLength(200).HasDefaultValue("");
                entity.Property(e => e.Cor).HasMaxLength(7);
                entity.Property(e => e.Nivel).IsRequired();
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            modelBuilder.Entity<StatusProjetoModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descricao).HasMaxLength(200).HasDefaultValue("");
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            modelBuilder.Entity<StatusUsuarioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descricao).HasMaxLength(200).HasDefaultValue("");
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            modelBuilder.Entity<TipoUsuarioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descricao).HasMaxLength(200).HasDefaultValue("");
                entity.Property(e => e.Nivel).IsRequired();
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            modelBuilder.Entity<TipoAtividadeModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(200).HasDefaultValue("");
                entity.Property(e => e.Cor).HasMaxLength(7);
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Equipes
            modelBuilder.Entity<EquipeModel>().HasData(
                new EquipeModel { Id = 1, Nome = "Frontend", Descricao = "Equipe responsável pelo desenvolvimento frontend", Ativo = true },
                new EquipeModel { Id = 2, Nome = "Backend", Descricao = "Equipe responsável pelo desenvolvimento backend", Ativo = true },
                new EquipeModel { Id = 3, Nome = "Mobile", Descricao = "Equipe responsável pelo desenvolvimento mobile", Ativo = true },
                new EquipeModel { Id = 4, Nome = "DevOps", Descricao = "Equipe responsável pela infraestrutura", Ativo = true }
            );

            // Seed Módulos
            modelBuilder.Entity<ModuloSistemaModel>().HasData(
                new ModuloSistemaModel { Id = 1, Nome = "Contábil", Descricao = "Módulo contábil do sistema", Ativo = true },
                new ModuloSistemaModel { Id = 2, Nome = "Financeiro", Descricao = "Módulo financeiro do sistema", Ativo = true },
                new ModuloSistemaModel { Id = 3, Nome = "Folha de Pagamento", Descricao = "Módulo de folha de pagamento", Ativo = true },
                new ModuloSistemaModel { Id = 4, Nome = "Vendas", Descricao = "Módulo de vendas", Ativo = true },
                new ModuloSistemaModel { Id = 5, Nome = "Marketing", Descricao = "Módulo de marketing", Ativo = true }
            );

            // Seed Status Usuario
            modelBuilder.Entity<StatusUsuarioModel>().HasData(
                new StatusUsuarioModel { Id = 1, Nome = "Ativo", Descricao = "Usuário ativo no sistema", Ativo = true },
                new StatusUsuarioModel { Id = 2, Nome = "Inativo", Descricao = "Usuário inativo no sistema", Ativo = true },
                new StatusUsuarioModel { Id = 3, Nome = "Bloqueado", Descricao = "Usuário bloqueado no sistema", Ativo = true }
            );

            // Seed Tipo Usuario
            modelBuilder.Entity<TipoUsuarioModel>().HasData(
                new TipoUsuarioModel { Id = 1, Nome = "Admin", Descricao = "Administrador do sistema", Nivel = 4, Ativo = true },
                new TipoUsuarioModel { Id = 2, Nome = "PO", Descricao = "Product Owner", Nivel = 3, Ativo = true },
                new TipoUsuarioModel { Id = 3, Nome = "Dev", Descricao = "Desenvolvedor", Nivel = 2, Ativo = true },
                new TipoUsuarioModel { Id = 4, Nome = "Viewer", Descricao = "Visualizador apenas", Nivel = 1, Ativo = true }
            );

            // Seed Criticidade
            modelBuilder.Entity<CriticidadeModel>().HasData(
                new CriticidadeModel { Id = 1, Nome = "Baixa", Descricao = "Criticidade baixa", Cor = "#28a745", Nivel = 1, Ativo = true },
                new CriticidadeModel { Id = 2, Nome = "Média", Descricao = "Criticidade média", Cor = "#ffc107", Nivel = 2, Ativo = true },
                new CriticidadeModel { Id = 3, Nome = "Alta", Descricao = "Criticidade alta", Cor = "#fd7e14", Nivel = 3, Ativo = true },
                new CriticidadeModel { Id = 4, Nome = "Crítica", Descricao = "Criticidade crítica", Cor = "#dc3545", Nivel = 4, Ativo = true }
            );

            // Seed Status Projeto
            modelBuilder.Entity<StatusProjetoModel>().HasData(
                new StatusProjetoModel { Id = 1, Nome = "Planejado", Descricao = "Projeto em planejamento", Ativo = true },
                new StatusProjetoModel { Id = 2, Nome = "Em Andamento", Descricao = "Projeto em execução", Ativo = true },
                new StatusProjetoModel { Id = 3, Nome = "Concluído", Descricao = "Projeto concluído", Ativo = true },
                new StatusProjetoModel { Id = 4, Nome = "Cancelado", Descricao = "Projeto cancelado", Ativo = true }
            );

            // Seed Tipo Atividade
            modelBuilder.Entity<TipoAtividadeModel>().HasData(
                new TipoAtividadeModel { Id = 1, Nome = "Desenvolvimento", Descricao = "Atividade de desenvolvimento", Cor = "#007bff", Ativo = true },
                new TipoAtividadeModel { Id = 2, Nome = "Teste", Descricao = "Atividade de teste", Cor = "#6c757d", Ativo = true },
                new TipoAtividadeModel { Id = 3, Nome = "Documentação", Descricao = "Atividade de documentação", Cor = "#17a2b8", Ativo = true },
                new TipoAtividadeModel { Id = 4, Nome = "Revisão", Descricao = "Atividade de revisão", Cor = "#20c997", Ativo = true }
            );

            // Seed Usuários iniciais
            modelBuilder.Entity<UsuarioModel>().HasData(
                new UsuarioModel
                {
                    Id = 1,
                    Nome = "Administrador",
                    Email = "admin@cygnusnet.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    EquipeId = null,
                    TipoUsuarioId = (int)TipoUsuario.Admin,
                    StatusUsuarioId = (int)StatusUsuario.Ativo,
                    BloqueadoPorRedefinicao = false,
                    DataCadastro = DateTime.Now
                },
                new UsuarioModel
                {
                    Id = 2,
                    Nome = "Marcos Araujo",
                    Email = "marcos@cygnusnet.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("marcos123"),
                    TipoUsuarioId = (int)TipoUsuario.PO,
                    StatusUsuarioId = (int)StatusUsuario.Ativo,
                    EquipeId = 1,
                    BloqueadoPorRedefinicao = false,
                    DataCadastro = DateTime.Now
                }
            );
        }
    }
}
