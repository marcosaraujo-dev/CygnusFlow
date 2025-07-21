using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class TipoUsuarioMapper
    {
        public static TipoUsuario ToDomain(TipoUsuarioModel model)
        {
            if (model == null) return null;

            return new TipoUsuario
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Nivel = model.Nivel,
                Ativo = model.Ativo
            };
        }

        public static TipoUsuarioModel ToModel(TipoUsuario domain)
        {
            if (domain == null) return null;

            return new TipoUsuarioModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Descricao = domain.Descricao,
                Nivel = domain.Nivel,
                Ativo = domain.Ativo
            };
        }
    }
}