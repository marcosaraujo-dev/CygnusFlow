using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Enums;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Entities
{
    public class Atividade
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public int ProjetoId { get; set; }
        public int ResponsavelId { get; set; }
        public TipoAtividade TipoAtividadeId { get; set; }
        public DateTime? DataInicioPlanejada { get; set; }
        public DateTime? DataFimPlanejada { get; set; }
        public DateTime? DataInicioReal { get; set; }
        public DateTime? DataFimReal { get; set; }
        public StatusProjeto StatusProjetoId { get; set; } = StatusProjeto.Planejado;
        public string? Observacoes { get; set; }
        public string? Impedimentos { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;


        public virtual Projeto Projeto { get; set; } = null!;
        public virtual Usuario Responsavel { get; set; } = null!;
        public virtual ICollection<AtividadeComentario> Comentarios { get; set; } = new List<AtividadeComentario>();

        public NotificationResult Validate()
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(Codigo))
                result.AddError(nameof(Codigo), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Código"), ErrorCodes.REQUIRED);
            else
            {
                var codigoValidation = CodigoAtividade.Create(Codigo);
                if (!codigoValidation.IsSuccess)
                    result.Merge(codigoValidation.Notifications);
            }

            if (string.IsNullOrWhiteSpace(Nome))
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Nome"), ErrorCodes.REQUIRED);
            else if (Nome.Length < 3)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MIN_LENGTH, "Nome", 3), ErrorCodes.MIN_LENGTH);
            else if (Nome.Length > 200)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Nome", 200), ErrorCodes.MAX_LENGTH);

            if (ProjetoId <= 0)
                result.AddError(nameof(ProjetoId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Projeto"), ErrorCodes.REQUIRED);

            if (ResponsavelId <= 0)
                result.AddError(nameof(ResponsavelId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Responsável"), ErrorCodes.REQUIRED);

            var datasValidation = ValidarDatas();
            result.Merge(datasValidation);

            var limitesValidation = ValidarObservacoes();
            result.Merge(limitesValidation);

            return result;
        }

        public NotificationResult ValidarDatas()
        {
            var result = new NotificationResult();

            // Validar datas planejadas
            if (DataInicioPlanejada.HasValue && DataFimPlanejada.HasValue)
            {
                if (DataFimPlanejada.Value <= DataInicioPlanejada.Value)
                    result.AddError(nameof(DataFimPlanejada), "Data fim planejada deve ser posterior à data início", ErrorCodes.PROJETO_DATAS_INVALIDAS);
            }

            // Validar datas reais
            if (DataInicioReal.HasValue && DataFimReal.HasValue)
            {
                if (DataFimReal.Value <= DataInicioReal.Value)
                    result.AddError(nameof(DataFimReal), "Data fim real deve ser posterior à data início", ErrorCodes.PROJETO_DATAS_INVALIDAS);
            }

            // Validar se datas reais não são futuras quando status é concluído
            if (StatusProjetoId == StatusProjeto.Concluido)
            {
                if (DataInicioReal.HasValue && DataInicioReal.Value.Date > DateTime.Now.Date)
                    result.AddError(nameof(DataInicioReal), "Data de início real não pode ser futura para atividade concluída", ErrorCodes.PROJETO_DATAS_INVALIDAS);

                if (DataFimReal.HasValue && DataFimReal.Value.Date > DateTime.Now.Date)
                    result.AddError(nameof(DataFimReal), "Data de fim real não pode ser futura para atividade concluída", ErrorCodes.PROJETO_DATAS_INVALIDAS);

                if (!DataFimReal.HasValue)
                    result.AddError(nameof(DataFimReal), "Data de fim real é obrigatória para atividades concluídas", ErrorCodes.REQUIRED);
            }

            return result;
        }

        public NotificationResult ValidarObservacoes()
        {
            var result = new NotificationResult();

            if (!string.IsNullOrEmpty(Observacoes) && Observacoes.Length > 2000)
                result.AddError(nameof(Observacoes), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Observações", 2000), ErrorCodes.MAX_LENGTH);

            if (!string.IsNullOrEmpty(Impedimentos) && Impedimentos.Length > 2000)
                result.AddError(nameof(Impedimentos), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Impedimentos", 2000), ErrorCodes.MAX_LENGTH);

            return result;
        }

        public NotificationResult ValidarDentroPeridoProjeto(DateTime dataInicioProjeto, DateTime dataFimProjeto)
        {
            var result = new NotificationResult();

            if (DataInicioPlanejada.HasValue)
            {
                if (DataInicioPlanejada.Value.Date < dataInicioProjeto.Date)
                    result.AddError(nameof(DataInicioPlanejada), ErrorMessages.GetMessage(ErrorCodes.ATIVIDADE_FORA_PERIODO_PROJETO), ErrorCodes.ATIVIDADE_FORA_PERIODO_PROJETO);
            }

            if (DataFimPlanejada.HasValue)
            {
                if (DataFimPlanejada.Value.Date > dataFimProjeto.Date)
                    result.AddError(nameof(DataFimPlanejada), ErrorMessages.GetMessage(ErrorCodes.ATIVIDADE_FORA_PERIODO_PROJETO), ErrorCodes.ATIVIDADE_FORA_PERIODO_PROJETO);
            }

            return result;
        }

        public bool EstaAtrasada()
        {
            if (StatusProjetoId == StatusProjeto.Concluido || StatusProjetoId == StatusProjeto.Cancelado)
                return false;

            var dataReferencia = DataFimPlanejada ?? DataFimReal;
            return dataReferencia.HasValue && DateTime.Now.Date > dataReferencia.Value.Date;
        }

        public TimeSpan? CalcularDuracaoPlanejada()
        {
            if (DataInicioPlanejada.HasValue && DataFimPlanejada.HasValue)
                return DataFimPlanejada.Value - DataInicioPlanejada.Value;
            return null;
        }

        public TimeSpan? CalcularDuracaoReal()
        {
            if (DataInicioReal.HasValue && DataFimReal.HasValue)
                return DataFimReal.Value - DataInicioReal.Value;
            return null;
        }
    }
}
