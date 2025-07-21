using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class ProjetoComentarioMapper
    {
        public static ProjetoComentario ToDomain(ProjetoComentarioModel model)
        {
            if (model == null) return null;

            return new ProjetoComentario
            {
                Id = model.Id,
                ProjetoId = model.ProjetoId,
                UsuarioId = model.UsuarioId,
                Comentario = model.Comentario,
                DataComentario = model.DataComentario
            };
        }

        public static ProjetoComentarioModel ToModel(ProjetoComentario domain)
        {
            if (domain == null) return null;

            return new ProjetoComentarioModel
            {
                Id = domain.Id,
                ProjetoId = domain.ProjetoId,
                UsuarioId = domain.UsuarioId,
                Comentario = domain.Comentario,
                DataComentario = domain.DataComentario
            };
        }

        public static void UpdateModel(ProjetoComentarioModel model, ProjetoComentario domain)
        {
            if (model == null || domain == null) return;

            model.Comentario = domain.Comentario;
            model.DataComentario = domain.DataComentario;
        }
    }
}