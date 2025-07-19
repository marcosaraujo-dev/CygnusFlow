using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Services;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        public Task<Result<string>> GerarCodigoRedefinicaoAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> GerarSenhaTemporariaAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UsuarioPodeAcessarProjetoAsync(int usuarioId, int projetoId)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarAlteracaoSenhaAsync(int usuarioId, string senhaAtual, string novaSenha)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarPermissoesAsync(Usuario usuario, string acao)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarSenhaAsync(string senha)
        {
            throw new NotImplementedException();
        }
    }
}
