using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class ModuloSistemaMapper
    {
        public static ModuloSistema ToDomain(ModuloSistemaModel model)
        {
            if (model == null) return null;

            return new ModuloSistema
            {
                Id = model.Id,
                Nome = model.Nome
            };
        }

        public static ModuloSistemaModel ToModel(ModuloSistema domain)
        {
            if (domain == null) return null;

            return new ModuloSistemaModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Descricao = "", // Valor padrão
                Ativo = true   // Valor padrão
            };
        }

        public static void UpdateModel(ModuloSistemaModel model, ModuloSistema domain)
        {
            if (model == null || domain == null) return;

            model.Nome = domain.Nome;
            // Mantém Descricao e Ativo existentes
        }
    }
}