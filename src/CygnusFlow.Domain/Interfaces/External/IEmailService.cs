using CygnusFlow.Domain.DTOs;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.External
{
    public interface IEmailService
    {
        Task<Result<bool>> EnviarEmailAsync(string destinatario, string assunto, string corpo);
        Task<Result<bool>> EnviarEmailHtmlAsync(string destinatario, string assunto, string corpoHtml);
        Task<Result<int>> EnviarEmailsLoteAsync(List<EmailDto> emails);
        Task<Result<bool>> EnviarCodigoRedefinicaoAsync(string email, string codigo, string nomeUsuario);
        Task<Result<bool>> EnviarNotificacaoProjetoAsync(string email, string nomeProjeto, string tipoNotificacao);
    }

    
}
