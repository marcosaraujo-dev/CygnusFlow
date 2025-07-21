using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class AtividadeComentarioMapper
    {
        public static AtividadeComentario ToDomain(AtividadeComentarioModel model)
        {
            if (model == null) return null;

            return new AtividadeComentario
            {
                Id = model.Id,
                AtividadeId = model.AtividadeId,
                UsuarioId = model.UsuarioId,
                Comentario = model.Comentario,
                DataComentario = model.DataComentario
            };
        }

        public static AtividadeComentarioModel ToModel(AtividadeComentario domain)
        {
            if (domain == null) return null;

            return new AtividadeComentarioModel
            {
                Id = domain.Id,
                AtividadeId = domain.AtividadeId,
                UsuarioId = domain.UsuarioId,
                Comentario = domain.Comentario,
                DataComentario = domain.DataComentario
            };
        }

        public static void UpdateModel(AtividadeComentarioModel model, AtividadeComentario domain)
        {
            if (model == null || domain == null) return;

            model.Comentario = domain.Comentario;
            model.DataComentario = domain.DataComentario;
        }
    }
}
