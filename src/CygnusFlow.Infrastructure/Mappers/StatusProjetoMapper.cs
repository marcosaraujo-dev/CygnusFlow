using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class StatusProjetoMapper
    {
        public static StatusProjeto ToDomain(StatusProjetoModel model)
        {
            if (model == null) return null;

            return new StatusProjeto
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Ativo = model.Ativo
            };
        }

        public static StatusProjetoModel ToModel(StatusProjeto domain)
        {
            if (domain == null) return null;

            return new StatusProjetoModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Descricao = domain.Descricao,
                Ativo = domain.Ativo
            };
        }
    }
}