using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Infrastructure.Data.Context
{
    public class CygnusFlowContext : DbContext
    {
        public CygnusFlowContext(DbContextOptions<CygnusFlowContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Atividade> Atividades { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<ModuloSistema> ModulosSistema { get; set; }
        public DbSet<ProjetoComentario> ProjetoComentarios { get; set; }
        public DbSet<AtividadeComentario> AtividadeComentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.SenhaHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.TipoUsuarioId).HasConversion<int>();
                entity.Property(e => e.StatusUsuarioId).HasConversion<int>();
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");
            });

            // Configuração Projeto
            modelBuilder.Entity<Projeto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CriticidadeId).HasConversion<int>();
                entity.Property(e => e.StatusProjetoId).HasConversion<int>();
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(p => p.Modulo)
                    .WithMany()
                    .HasForeignKey(p => p.ModuloId);
            });

            // Configuração Atividade
            modelBuilder.Entity<Atividade>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StatusProjetoId).HasConversion<int>();
                entity.Property(e => e.TipoAtividadeId).HasConversion<int>();
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(a => a.Projeto)
                    .WithMany(p => p.Atividades)
                    .HasForeignKey(a => a.ProjetoId);

                entity.HasOne(a => a.Responsavel)
                    .WithMany(u => u.Atividades)
                    .HasForeignKey(a => a.ResponsavelId);
            });

            // Configurações auxiliares
            modelBuilder.Entity<Equipe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<ModuloSistema>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Equipes
            modelBuilder.Entity<Equipe>().HasData(
                new Equipe { Id = 1, Nome = "Frontend" },
                new Equipe { Id = 2, Nome = "Backend" },
                new Equipe { Id = 3, Nome = "Mobile" },
                new Equipe { Id = 4, Nome = "DevOps" }
            );

            // Seed Módulos
            modelBuilder.Entity<ModuloSistema>().HasData(
                new ModuloSistema { Id = 1, Nome = "Contábil" },
                new ModuloSistema { Id = 2, Nome = "Financeiro" },
                new ModuloSistema { Id = 3, Nome = "Folha de Pagamento" },
                new ModuloSistema { Id = 4, Nome = "Vendas" },
                new ModuloSistema { Id = 5, Nome = "Marketing" }
            );

            // Seed Usuários iniciais
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Administrador",
                    Email = "admin@cygnusnet.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    TipoUsuarioId = TipoUsuario.Admin,
                    StatusUsuarioId = StatusUsuario.Ativo,
                    DataCadastro = DateTime.Now
                },
                new Usuario
                {
                    Id = 2,
                    Nome = "Marcos Araujo",
                    Email = "marcos@cygnusnet.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("marcos123"),
                    TipoUsuarioId = TipoUsuario.PO,
                    StatusUsuarioId = StatusUsuario.Ativo,
                    EquipeId = 1,
                    DataCadastro = DateTime.Now
                }
            );
        }
    }
}
