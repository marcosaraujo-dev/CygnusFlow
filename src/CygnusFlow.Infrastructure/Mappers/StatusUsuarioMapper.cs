using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class StatusUsuarioMapper
    {
        public static StatusUsuario ToDomain(StatusUsuarioModel model)
        {
            if (model == null) return null;

            return new StatusUsuario
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Ativo = model.Ativo
            };
        }

        public static StatusUsuarioModel ToModel(StatusUsuario domain)
        {
            if (domain == null) return null;

            return new StatusUsuarioModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Descricao = domain.Descricao,
                Ativo = domain.Ativo
            };
        }
    }
}