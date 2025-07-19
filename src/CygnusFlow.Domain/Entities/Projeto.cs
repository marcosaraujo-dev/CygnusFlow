using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Enums;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace CygnusFlow.Domain.Entities
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public int ModuloId { get; set; }
        public Criticidade CriticidadeId { get; set; }
        public DateTime DataInicioPO { get; set; }
        public DateTime DataFimPO { get; set; }
        public int EstimativaHoras { get; set; }
        public StatusProjeto StatusProjetoId { get; set; } = StatusProjeto.Planejado;
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ModuloSistema? Modulo { get; set; }
        public virtual ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
        public virtual ICollection<ProjetoComentario> Comentarios { get; set; } = new List<ProjetoComentario>();

        public NotificationResult Validate()
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(Codigo))
                result.AddError(nameof(Codigo), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Código"), ErrorCodes.REQUIRED);
            else
            {
                var codigoValidation = CodigoProjeto.Create(Codigo);
                if (!codigoValidation.IsSuccess)
                    result.Merge(codigoValidation.Notifications);
            }

            if (string.IsNullOrWhiteSpace(Nome))
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Nome"), ErrorCodes.REQUIRED);
            else if (Nome.Length < 3)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MIN_LENGTH, "Nome", 3), ErrorCodes.MIN_LENGTH);
            else if (Nome.Length > 200)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Nome", 200), ErrorCodes.MAX_LENGTH);

            if (ModuloId <= 0)
                result.AddError(nameof(ModuloId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Módulo"), ErrorCodes.REQUIRED);

            if (EstimativaHoras <= 0)
                result.AddError(nameof(EstimativaHoras), "Estimativa deve ser maior que zero", ErrorCodes.INVALID_FORMAT);

            var periodoValidation = ValidarPrazos();
            result.Merge(periodoValidation);

            return result;
        }

        public NotificationResult ValidarPrazos()
        {
            var result = new NotificationResult();

            if (DataInicioPO == default)
                result.AddError(nameof(DataInicioPO), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Data de Início P.O."), ErrorCodes.REQUIRED);

            if (DataFimPO == default)
                result.AddError(nameof(DataFimPO), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Data de Fim P.O."), ErrorCodes.REQUIRED);

            if (DataInicioPO != default && DataFimPO != default && DataFimPO <= DataInicioPO)
                result.AddError(nameof(DataFimPO), ErrorMessages.GetMessage(ErrorCodes.PROJETO_DATA_FIM_ANTERIOR_INICIO), ErrorCodes.PROJETO_DATA_FIM_ANTERIOR_INICIO);

            return result;
        }

        public TimeSpan CalcularDuracao() => DataFimPO - DataInicioPO;

        public bool EstaAtrasado() => StatusProjetoId != StatusProjeto.Concluido && DateTime.Now.Date > DataFimPO.Date;
    }
}
