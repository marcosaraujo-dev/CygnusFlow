using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Shared;
using System;

namespace CygnusFlow.Domain.Entities
{
    public class AtividadeComentario
    {
        public int Id { get; set; }
        public int AtividadeId { get; set; }
        public int UsuarioId { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime DataComentario { get; set; } = DateTime.Now;

        public virtual Atividade Atividade { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;

        public NotificationResult Validate()
        {
            var result = new NotificationResult();

            if (AtividadeId <= 0)
                result.AddError(nameof(AtividadeId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Atividade"), ErrorCodes.REQUIRED);

            if (UsuarioId <= 0)
                result.AddError(nameof(UsuarioId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Usuário"), ErrorCodes.REQUIRED);

            if (string.IsNullOrWhiteSpace(Comentario))
                result.AddError(nameof(Comentario), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Comentário"), ErrorCodes.REQUIRED);
            else if (Comentario.Length < 5)
                result.AddError(nameof(Comentario), ErrorMessages.GetMessage(ErrorCodes.MIN_LENGTH, "Comentário", 5), ErrorCodes.MIN_LENGTH);
            else if (Comentario.Length > 2000)
                result.AddError(nameof(Comentario), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Comentário", 2000), ErrorCodes.MAX_LENGTH);

            if (DataComentario == default)
                result.AddError(nameof(DataComentario), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Data do Comentário"), ErrorCodes.REQUIRED);
            else if (DataComentario > DateTime.Now.AddMinutes(5)) // Permitir pequena margem para diferenças de relógio
                result.AddError(nameof(DataComentario), "Data do comentário não pode ser futura", ErrorCodes.INVALID_FORMAT);

            return result;
        }

        public bool PodeSerEditadoPor(int usuarioId, bool isAdmin = false)
        {
            // Apenas o autor ou admin pode editar
            if (isAdmin) return true;

            // Só pode editar se for o autor e o comentário foi feito há menos de 24h
            return UsuarioId == usuarioId && DateTime.Now.Subtract(DataComentario).TotalHours <= 24;
        }

        public bool PodeSerExcluidoPor(int usuarioId, bool isAdmin = false)
        {
            // Admin sempre pode excluir
            if (isAdmin) return true;

            // Autor pode excluir se foi feito há menos de 1h
            return UsuarioId == usuarioId && DateTime.Now.Subtract(DataComentario).TotalHours <= 1;
        }
    }
}
