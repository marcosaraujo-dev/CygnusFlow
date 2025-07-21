using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class UsuarioMapper
    {
        public static Usuario ToDomain(UsuarioModel model)
        {
            if (model == null) return null;

            return new Usuario
            {
                Id = model.Id,
                Nome = model.Nome,
                Email = model.Email,
                SenhaHash = model.SenhaHash,
                EquipeId = model.EquipeId,
                TipoUsuarioId = model.TipoUsuarioId,
                StatusUsuarioId = model.StatusUsuarioId,
                BloqueadoPorRedefinicao = model.BloqueadoPorRedefinicao,
                DataCadastro = model.DataCadastro
            };
        }

        public static UsuarioModel ToModel(Usuario domain)
        {
            if (domain == null) return null;

            return new UsuarioModel
            {
                Id = domain.Id,
                Nome = domain.Nome,
                Email = domain.Email,
                SenhaHash = domain.SenhaHash,
                EquipeId = domain.EquipeId,
                TipoUsuarioId = domain.TipoUsuarioId,
                StatusUsuarioId = domain.StatusUsuarioId,
                BloqueadoPorRedefinicao = domain.BloqueadoPorRedefinicao,
                DataCadastro = domain.DataCadastro
            };
        }
        public static void UpdateModel(UsuarioModel model, Usuario domain)
        {
            if (model == null || domain == null) return;

            model.Nome = domain.Nome;
            model.Email = domain.Email;
            model.SenhaHash = domain.SenhaHash;
            model.EquipeId = domain.EquipeId;
            model.TipoUsuarioId = domain.TipoUsuarioId;
            model.StatusUsuarioId = domain.StatusUsuarioId;
            model.BloqueadoPorRedefinicao = domain.BloqueadoPorRedefinicao;
        }
    }
}