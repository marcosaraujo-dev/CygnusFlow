using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class CriticidadeMapper
    {
        public static Criticidade ToDomain(CriticidadeModel model)
        {
            if (model == null) return null;

            return new Criticidade
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Cor = model.Cor,
                Nivel = model.Nivel,
                Ativo = model.Ativo
            };
        }

        public static CriticidadeModel ToModel(Criticidade domain)
        {
            if (domain == null) return null;

            return new CriticidadeModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Descricao = domain.Descricao,
                Cor = domain.Cor,
                Nivel = domain.Nivel,
                Ativo = domain.Ativo
            };
        }
    }
}