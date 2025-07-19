using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace CygnusFlow.Domain.Interfaces.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioRepository Usuarios { get; }
        IProjetoRepository Projetos { get; }
        IAtividadeRepository Atividades { get; }
        IProjetoComentarioRepository ProjetoComentarios { get; }
        IAtividadeComentarioRepository AtividadeComentarios { get; }
        IEquipeRepository Equipes { get; }
        IModuloSistemaRepository ModulosSistema { get; }

        // Transaction methods
        Task<Result<int>> SaveChangesAsync();
        Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
