using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace CygnusFlow.Domain.Entities
{

    public class Projeto
    {
        public int Id { get; private set; }

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(20, ErrorMessage = "Código deve ter no máximo 20 caracteres")]
        public string Codigo { get; private set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 255 caracteres")]
        public string Nome { get; private set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }

        public int ModuloId { get; private set; }
        public virtual ModuloSistema Modulo { get; set; } = null!;
        public int CriticidadeId { get; set; }
        public virtual Criticidade Criticidade { get; set; } = null!;

        public DateTime DataInicioPO { get; private set; }
        public DateTime DataFimPO { get; private set; }

        // Datas Time
        public DateTime? DataInicioTime { get; set; }
        public DateTime? DataFimTime { get; set; }

        // Datas Reais
        public DateTime? DataInicioReal { get; set; }
        public DateTime? DataFimReal { get; set; }

        public int EstimativaHoras { get; set; }
        public int? HorasTrabalhadasReal { get; set; }

        public int StatusProjetoId { get; set; }
        public virtual StatusProjeto StatusProjeto { get; set; } = null!;


        public int? ResponsavelId { get; set; }
        public virtual Usuario? Responsavel { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataUltimaAtualizacao { get; set; }



        // Coleções para regras de negócio (não são propriedades de navegação do EF)
        private readonly List<Atividade> _atividades = new();
        private readonly List<ProjetoComentario> _comentarios = new();

        [StringLength(2000, ErrorMessage = "Observações deve ter no máximo 2000 caracteres")]
        public string? Observacoes { get; set; }

        public IReadOnlyCollection<Atividade> Atividades => _atividades.AsReadOnly();
        public IReadOnlyCollection<ProjetoComentario> Comentarios => _comentarios.AsReadOnly();

        // Construtor protegido para repositório/infraestrutura
        protected Projeto() { }

        // Construtor para criação de domínio
        public Projeto(string codigo, string nome, int moduloId, int criticidadeId,
                      DateTime dataInicioPO, DateTime dataFimPO, int estimativaHoras)
        {
            Codigo = codigo?.Trim().ToUpper() ?? string.Empty;
            Nome = nome?.Trim() ?? string.Empty;
            ModuloId = moduloId;
            CriticidadeId = criticidadeId;
            DataInicioPO = dataInicioPO.Date;
            DataFimPO = dataFimPO.Date;
            EstimativaHoras = estimativaHoras;
            DataCadastro = DateTime.Now;
        }

        // Propriedades calculadas de negócio
        public int DuracaoEmDias => (DataFimPO - DataInicioPO).Days + 1;

        public double PercentualConclusao
        {
            get
            {
                if (!_atividades.Any()) return 0;
                var atividadesConcluidas = _atividades.Count(a => a.StatusProjetoId == 3); // 3 = Concluído
                return (double)atividadesConcluidas / _atividades.Count * 100;
            }
        }

        public int HorasRealizadas => _atividades.Sum(a => a.CalcularHorasRealizadas());

        public bool EstaAtrasado => DateTime.Now.Date > DataFimPO && StatusProjetoId != (int) Enums.StatusProjeto.Concluido;

        public TimeSpan CalcularDuracao() => DataFimPO - DataInicioPO;

        // Métodos de negócio
        public NotificationResult DefinirCodigo(string codigo)
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(codigo))
                result.AddError(nameof(Codigo), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Código"), ErrorCodes.REQUIRED);
            else
            {
                var codigoValidation = CodigoProjeto.Create(codigo);
                if (!codigoValidation.IsSuccess)
                    result.Merge(codigoValidation.Notifications);
                else
                    Codigo = codigo.Trim().ToUpper();
            }

            return result;
        }

        public NotificationResult DefinirNome(string nome)
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(nome))
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Nome"), ErrorCodes.REQUIRED);
            else if (nome.Length < 3)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MIN_LENGTH, "Nome", 3), ErrorCodes.MIN_LENGTH);
            else if (nome.Length > 200)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Nome", 200), ErrorCodes.MAX_LENGTH);
            else
                Nome = nome.Trim();

            return result;
        }

        public NotificationResult AlterarPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var result = new NotificationResult();

            if (dataInicio == default)
                result.AddError(nameof(DataInicioPO), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Data de Início P.O."), ErrorCodes.REQUIRED);

            if (dataFim == default)
                result.AddError(nameof(DataFimPO), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Data de Fim P.O."), ErrorCodes.REQUIRED);

            if (dataInicio != default && dataFim != default && dataFim <= dataInicio)
                result.AddError(nameof(DataFimPO), ErrorMessages.GetMessage(ErrorCodes.PROJETO_DATA_FIM_ANTERIOR_INICIO), ErrorCodes.PROJETO_DATA_FIM_ANTERIOR_INICIO);

            if (result.IsValid)
            {
                DataInicioPO = dataInicio.Date;
                DataFimPO = dataFim.Date;
            }

            return result;
        }

        public NotificationResult AlterarEstimativaHoras(int estimativaHoras)
        {
            var result = new NotificationResult();

            if (estimativaHoras <= 0)
                result.AddError(nameof(EstimativaHoras), "Estimativa deve ser maior que zero", ErrorCodes.INVALID_FORMAT);
            else if (estimativaHoras > 10000)
                result.AddError(nameof(EstimativaHoras), "Estimativa não pode exceder 10.000 horas", ErrorCodes.INVALID_FORMAT);
            else
                EstimativaHoras = estimativaHoras;

            return result;
        }

        public NotificationResult AlterarStatus(int statusId)
        {
            var result = new NotificationResult();

            if (statusId <= 0)
                result.AddError(nameof(StatusProjetoId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Status"), ErrorCodes.REQUIRED);
            else
                StatusProjetoId = statusId;

            return result;
        }

        public NotificationResult AlterarCodigo(string codigo)
        {
            var result = new NotificationResult();

            if ( String.IsNullOrEmpty(codigo))
                result.AddError(nameof(StatusProjetoId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Codigo"), ErrorCodes.REQUIRED);
            else
                Codigo = codigo;

            return result;
        }

        public NotificationResult AdicionarAtividade(Atividade atividade)
        {
            var result = new NotificationResult();

            if (atividade == null)
            {
                result.AddError("Atividade", "Atividade não pode ser nula", "NULL_VALUE");
                return result;
            }

            var validacaoAtividade = atividade.Validate();
            if (!validacaoAtividade.IsValid)
            {
                result.Merge(validacaoAtividade);
                return result;
            }

            // Validar se atividade está dentro do período do projeto
            var validacaoPeriodo = atividade.ValidarDentroPeridoProjeto(DataInicioPO, DataFimPO);
            if (!validacaoPeriodo.IsValid)
            {
                result.Merge(validacaoPeriodo);
                return result;
            }

            _atividades.Add(atividade);
            return result;
        }

        public NotificationResult AdicionarComentario(ProjetoComentario comentario)
        {
            var result = new NotificationResult();

            if (comentario == null)
            {
                result.AddError("Comentario", "Comentário não pode ser nulo", "NULL_VALUE");
                return result;
            }

            var validacao = comentario.Validate();
            if (!validacao.IsValid)
            {
                result.Merge(validacao);
                return result;
            }

            _comentarios.Add(comentario);
            return result;
        }

        public IEnumerable<Atividade> ObterAtividadesPorStatus(int statusId)
        {
            return _atividades.Where(a => a.StatusProjetoId == statusId);
        }

        public int CalcularAtrasoEmDias()
        {

            if (!DataFimReal.HasValue)
            {
                // Se ainda não finalizou, calcula com base na data atual
                var dataReferencia = DateTime.Now.Date;
                var dataLimite = DataFimTime ?? DataFimPO;

                if (dataReferencia > dataLimite.Date)
                    return (dataReferencia - dataLimite.Date).Days;
            }
            else
            {
                // Se já finalizou, calcula com base na data real de conclusão
                var dataLimite = DataFimTime ?? DataFimPO;
                if (DataFimReal.Value.Date > dataLimite.Date)
                    return (DataFimReal.Value.Date - dataLimite.Date).Days;
            }


            return 0;
        }

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

            if (CriticidadeId <= 0)
                result.AddError(nameof(CriticidadeId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Criticidade"), ErrorCodes.REQUIRED);

            if (EstimativaHoras <= 0)
                result.AddError(nameof(EstimativaHoras), "Estimativa deve ser maior que zero", ErrorCodes.INVALID_FORMAT);
            else if (EstimativaHoras > 10000)
                result.AddError(nameof(EstimativaHoras), "Estimativa não pode exceder 10.000 horas", ErrorCodes.INVALID_FORMAT);

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

            // Validar coerência entre datas P.O.
            if (DataFimPO <= DataInicioPO)
                result.AddError("DatasPO", "Data fim P.O. deve ser posterior à data início P.O.");

            // Validar coerência entre datas Time (se informadas)
            if (DataInicioTime.HasValue && DataFimTime.HasValue && DataFimTime <= DataInicioTime)
                result.AddError("DatasTime", "Data fim Time deve ser posterior à data início Time");

            // Validar coerência entre datas Reais (se informadas)
            if (DataInicioReal.HasValue && DataFimReal.HasValue && DataFimReal <= DataInicioReal)
                result.AddError("DatasReais", "Data fim Real deve ser posterior à data início Real");

            // Validar se data real não é anterior à data P.O.
            if (DataInicioReal.HasValue && DataInicioReal < DataInicioPO.Date)
                result.AddWarning("DataInicioReal", "Data início real é anterior à data prevista pelo P.O.");


            return result;
        }

        // Método para carregar dados do repositório (usado pela infraestrutura)
        public void CarregarDados(int id, string codigo, string nome, int moduloId, int criticidadeId,
                                 DateTime dataInicioPO, DateTime dataFimPO, int estimativaHoras,
                                 int statusProjetoId, DateTime dataCadastro)
        {
            Id = id;
            Codigo = codigo;
            Nome = nome;
            ModuloId = moduloId;
            CriticidadeId = criticidadeId;
            DataInicioPO = dataInicioPO;
            DataFimPO = dataFimPO;
            EstimativaHoras = estimativaHoras;
            StatusProjetoId = statusProjetoId;
            DataCadastro = dataCadastro;
        }

        public void CarregarAtividades(IEnumerable<Atividade> atividades)
        {
            _atividades.Clear();
            _atividades.AddRange(atividades);
        }

        public void CarregarComentarios(IEnumerable<ProjetoComentario> comentarios)
        {
            _comentarios.Clear();
            _comentarios.AddRange(comentarios);
        }
    }
}
