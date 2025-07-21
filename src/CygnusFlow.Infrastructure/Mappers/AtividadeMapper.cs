using CygnusFlow.Domain.Entities;
using CygnusFlow.Infrastructure.Data.Models;
using System.Linq;

namespace CygnusFlow.Infrastructure.Mappers
{
    public static class AtividadeMapper
    {
        // Converter Model EF para Entidade de Domínio
        public static Atividade ToDomain(AtividadeModel model)
        {
            if (model == null) return null;

            var atividade = new Atividade(
                codigo: model.Codigo,
                nome: model.Nome,
                projetoId: model.ProjetoId,
                responsavelId: model.ResponsavelId,
                tipoAtividadeId: model.TipoAtividadeId
            );

            // Carregar dados completos
            atividade.CarregarDados(
                id: model.Id,
                codigo: model.Codigo,
                nome: model.Nome,
                projetoId: model.ProjetoId,
                responsavelId: model.ResponsavelId,
                tipoAtividadeId: model.TipoAtividadeId,
                dataInicioPlanejada: model.DataInicioPlanejada,
                dataFimPlanejada: model.DataFimPlanejada,
                dataInicioReal: model.DataInicioReal,
                dataFimReal: model.DataFimReal,
                statusProjetoId: model.StatusProjetoId,
                observacoes: model.Observacoes,
                impedimentos: model.Impedimentos,
                dataCadastro: model.DataCadastro
            );

            // Carregar comentários relacionados se existirem
            if (model.Comentarios?.Any() == true)
            {
                var comentarios = model.Comentarios.Select(AtividadeComentarioMapper.ToDomain).ToList();
                atividade.CarregarComentarios(comentarios);
            }

            return atividade;
        }

        // Converter Entidade de Domínio para Model EF
        public static AtividadeModel ToModel(Atividade domain)
        {
            if (domain == null) return null;

            return new AtividadeModel
            {
                Id = domain.Id,
                Codigo = domain.Codigo,
                Nome = domain.Nome,
                ProjetoId = domain.ProjetoId,
                ResponsavelId = domain.ResponsavelId,
                TipoAtividadeId = domain.TipoAtividadeId,
                DataInicioPlanejada = domain.DataInicioPlanejada,
                DataFimPlanejada = domain.DataFimPlanejada,
                DataInicioReal = domain.DataInicioReal,
                DataFimReal = domain.DataFimReal,
                StatusProjetoId = domain.StatusProjetoId,
                Observacoes = domain.Observacoes,
                Impedimentos = domain.Impedimentos,
                DataCadastro = domain.DataCadastro
            };
        }

        // Atualizar Model existente com dados da Entidade de Domínio
        public static void UpdateModel(AtividadeModel model, Atividade domain)
        {
            if (model == null || domain == null) return;

            model.Codigo = domain.Codigo;
            model.Nome = domain.Nome;
            model.ResponsavelId = domain.ResponsavelId;
            model.TipoAtividadeId = domain.TipoAtividadeId;
            model.DataInicioPlanejada = domain.DataInicioPlanejada;
            model.DataFimPlanejada = domain.DataFimPlanejada;
            model.DataInicioReal = domain.DataInicioReal;
            model.DataFimReal = domain.DataFimReal;
            model.StatusProjetoId = domain.StatusProjetoId;
            model.Observacoes = domain.Observacoes;
            model.Impedimentos = domain.Impedimentos;
        }
    }
}