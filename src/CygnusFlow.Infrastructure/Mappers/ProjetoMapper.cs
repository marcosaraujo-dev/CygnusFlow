using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;
using System.Linq;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class ProjetoMapper
    {
        
        public static Projeto ToDomain(ProjetoModel model)
        {
            if (model == null) return null;

            var projeto = new Projeto(
                codigo: model.Codigo,
                nome: model.Nome,
                moduloId: model.ModuloId,
                criticidadeId: model.CriticidadeId,
                dataInicioPO: model.DataInicioPO,
                dataFimPO: model.DataFimPO,
                estimativaHoras: model.EstimativaHoras
            );

            // Carregar dados que não vêm do construtor
            projeto.CarregarDados(
                id: model.Id,
                codigo: model.Codigo,
                nome: model.Nome,
                moduloId: model.ModuloId,
                criticidadeId: model.CriticidadeId,
                dataInicioPO: model.DataInicioPO,
                dataFimPO: model.DataFimPO,
                estimativaHoras: model.EstimativaHoras,
                statusProjetoId: model.StatusProjetoId,
                dataCadastro: model.DataCadastro
            );

            // Carregar atividades relacionadas se existirem
            if (model.Atividades?.Any() == true)
            {
                var atividades = model.Atividades.Select(AtividadeMapper.ToDomain).ToList();
                projeto.CarregarAtividades(atividades);
            }

            // Carregar comentários relacionados se existirem
            if (model.Comentarios?.Any() == true)
            {
                var comentarios = model.Comentarios.Select(ProjetoComentarioMapper.ToDomain).ToList();
                projeto.CarregarComentarios(comentarios);
            }

            return projeto;
        }

        // Converter Entidade de Domínio para Model EF
        public static ProjetoModel ToModel(Projeto domain)
        {
            if (domain == null) return null;

            return new ProjetoModel
            {
                Id = domain.Id,
                Codigo = domain.Codigo,
                Nome = domain.Nome,
                ModuloId = domain.ModuloId,
                CriticidadeId = domain.CriticidadeId,
                DataInicioPO = domain.DataInicioPO,
                DataFimPO = domain.DataFimPO,
                EstimativaHoras = domain.EstimativaHoras,
                StatusProjetoId = domain.StatusProjetoId,
                DataCadastro = domain.DataCadastro
            };
        }

        // Atualizar Model existente com dados da Entidade de Domínio
        public static void UpdateModel(ProjetoModel model, Projeto domain)
        {
            if (model == null || domain == null) return;

            model.Codigo = domain.Codigo;
            model.Nome = domain.Nome;
            model.ModuloId = domain.ModuloId;
            model.CriticidadeId = domain.CriticidadeId;
            model.DataInicioPO = domain.DataInicioPO;
            model.DataFimPO = domain.DataFimPO;
            model.EstimativaHoras = domain.EstimativaHoras;
            model.StatusProjetoId = domain.StatusProjetoId;
        }
    }
}
