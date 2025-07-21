using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class EquipeMapper
    {
        public static Equipe ToDomain(EquipeModel model)
        {
            if (model == null) return null;

            return new Equipe
            {
                Id = model.Id,
                Nome = model.Nome
            };
        }

        public static EquipeModel ToModel(Equipe domain)
        {
            if (domain == null) return null;

            return new EquipeModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Descricao = "", // Valor padrão pois a entidade de domínio não tem Descricao
                Ativo = true   // Valor padrão
            };
        }

        public static void UpdateModel(EquipeModel model, Equipe domain)
        {
            if (model == null || domain == null) return;

            model.Nome = domain.Nome;
            // Mantém Descricao e Ativo existentes
        }
    }
}