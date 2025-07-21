using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class TipoAtividadeMapper
    {
        public static TipoAtividade ToDomain(TipoAtividadeModel model)
        {
            if (model == null) return null;

            return new TipoAtividade
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Cor = model.Cor,
                Ativo = model.Ativo
            };
        }

        public static TipoAtividadeModel ToModel(TipoAtividade domain)
        {
            if (domain == null) return null;

            return new TipoAtividadeModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Descricao = domain.Descricao,
                Cor = domain.Cor,
                Ativo = domain.Ativo
            };
        }
    }
}