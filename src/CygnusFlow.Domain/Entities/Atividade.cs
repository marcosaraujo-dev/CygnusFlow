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
        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Nome { get; private set; } = string.Empty;
        public int ProjetoId { get; private set; }
        public int ResponsavelId { get; private set; }
        public int TipoAtividadeId { get; private set; }
        public DateTime? DataInicioPlanejada { get; private set; }
        public DateTime? DataFimPlanejada { get; private set; }
        public DateTime? DataInicioReal { get; private set; }
        public DateTime? DataFimReal { get; private set; }

        public int EstimativaHoras { get; set; }
        public int? HorasTrabalhadasReal { get; set; }
        public int? HorasTrabalhadasPlanejada { get; set; } = 0; 
        public int StatusProjetoId { get; private set; } = 1;
        public string? Observacoes { get; private set; }
        public string? Impedimentos { get; private set; }
        public DateTime DataCadastro { get; private set; } = DateTime.Now;

        private readonly List<AtividadeComentario> _comentarios = new();
        public IReadOnlyCollection<AtividadeComentario> Comentarios => _comentarios.AsReadOnly();

        // Construtor protegido para repositório/infraestrutura
        protected Atividade() { }

        // Construtor para criação de domínio
        public Atividade(string codigo, string nome, int projetoId, int responsavelId, int tipoAtividadeId)
        {
            Codigo = codigo?.Trim().ToUpper() ?? string.Empty;
            Nome = nome?.Trim() ?? string.Empty;
            ProjetoId = projetoId;
            ResponsavelId = responsavelId;
            TipoAtividadeId = tipoAtividadeId;
            DataCadastro = DateTime.Now;
        }

        // Propriedades calculadas de negócio
        public bool EstaAtrasada => EstaAtrasadaMethod();
        public bool EstaEmAndamento => DataInicioReal.HasValue && !DataFimReal.HasValue;
        public bool EstaConcluida => DataFimReal.HasValue;
        public bool TemImpedimentos => !string.IsNullOrWhiteSpace(Impedimentos);

        public int CalcularHorasRealizadas()
        {
            if (DataInicioReal.HasValue && DataFimReal.HasValue)
            {
                var dias = (DataFimReal.Value - DataInicioReal.Value).Days + 1;
                return dias * 8; // Assumindo 8 horas por dia útil
            }
            return 0;
        }

        // Métodos de negócio
        public NotificationResult DefinirCodigo(string codigo)
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(codigo))
                result.AddError(nameof(Codigo), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Código"), ErrorCodes.REQUIRED);
            else
            {
                var codigoValidation = CodigoAtividade.Create(codigo);
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

        public NotificationResult AtualizarPlanejamento(DateTime? dataInicio, DateTime? dataFim)
        {
            var result = new NotificationResult();

            if (dataInicio.HasValue && dataFim.HasValue && dataFim.Value <= dataInicio.Value)
            {
                result.AddError(nameof(DataFimPlanejada), "Data fim planejada deve ser posterior à data início", ErrorCodes.PROJETO_DATAS_INVALIDAS);
                return result;
            }

            DataInicioPlanejada = dataInicio?.Date;
            DataFimPlanejada = dataFim?.Date;

            return result;
        }

        public NotificationResult IniciarAtividade()
        {
            var result = new NotificationResult();

            if (DataInicioReal.HasValue)
            {
                result.AddError(nameof(DataInicioReal), "Atividade já foi iniciada", "ALREADY_STARTED");
                return result;
            }

            if (StatusProjetoId == 3)
            {
                result.AddError(nameof(StatusProjetoId), "Não é possível iniciar uma atividade concluída", "INVALID_STATUS");
                return result;
            }

            DataInicioReal = DateTime.Now.Date;
            StatusProjetoId = 2; // Em Andamento

            return result;
        }

        public NotificationResult ConcluirAtividade()
        {
            var result = new NotificationResult();

            if (!DataInicioReal.HasValue)
            {
                result.AddError(nameof(DataInicioReal), "Atividade deve ser iniciada antes de ser concluída", "NOT_STARTED");
                return result;
            }

            if (DataFimReal.HasValue)
            {
                result.AddError(nameof(DataFimReal), "Atividade já foi concluída", "ALREADY_COMPLETED");
                return result;
            }

            DataFimReal = DateTime.Now.Date;
            StatusProjetoId = 3; // Concluído

            return result;
        }

        public NotificationResult AdicionarObservacao(string observacao)
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(observacao))
            {
                result.AddError(nameof(Observacoes), "Observação não pode estar vazia", ErrorCodes.REQUIRED);
                return result;
            }

            if (observacao.Length > 500)
            {
                result.AddError(nameof(Observacoes), "Observação individual não pode exceder 500 caracteres", ErrorCodes.MAX_LENGTH);
                return result;
            }

            var novaObservacao = $"--- {DateTime.Now:dd/MM/yyyy HH:mm} ---\n{observacao.Trim()}";

            if (string.IsNullOrWhiteSpace(Observacoes))
                Observacoes = novaObservacao;
            else
                Observacoes += $"\n\n{novaObservacao}";

            // Validar tamanho total
            if (Observacoes.Length > 2000)
            {
                result.AddError(nameof(Observacoes), "Total de observações não pode exceder 2000 caracteres", ErrorCodes.MAX_LENGTH);
            }

            return result;
        }

        public NotificationResult AdicionarImpedimento(string impedimento)
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(impedimento))
            {
                result.AddError(nameof(Impedimentos), "Impedimento não pode estar vazio", ErrorCodes.REQUIRED);
                return result;
            }

            if (impedimento.Length > 500)
            {
                result.AddError(nameof(Impedimentos), "Impedimento individual não pode exceder 500 caracteres", ErrorCodes.MAX_LENGTH);
                return result;
            }

            var novoImpedimento = $"--- {DateTime.Now:dd/MM/yyyy HH:mm} ---\n{impedimento.Trim()}";

            if (string.IsNullOrWhiteSpace(Impedimentos))
                Impedimentos = novoImpedimento;
            else
                Impedimentos += $"\n\n{novoImpedimento}";

            // Validar tamanho total
            if (Impedimentos.Length > 2000)
            {
                result.AddError(nameof(Impedimentos), "Total de impedimentos não pode exceder 2000 caracteres", ErrorCodes.MAX_LENGTH);
            }

            return result;
        }

        public void RemoverImpedimentos()
        {
            Impedimentos = null;
        }

        public NotificationResult AdicionarComentario(AtividadeComentario comentario)
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

        // Validações
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

            if (TipoAtividadeId <= 0)
                result.AddError(nameof(TipoAtividadeId), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Tipo de Atividade"), ErrorCodes.REQUIRED);

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
            if (StatusProjetoId == (int) Enums.StatusProjeto.Concluido)
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
        public int CalcularAtrasoEmDias()
        {
            if (!DataFimReal.HasValue)
            {
                var dataReferencia = DateTime.Now.Date;
                var dataLimite = DataFimPlanejada ?? DataFimReal;

                if (dataReferencia > dataLimite?.Date)
                    return (dataReferencia - dataLimite.Value.Date).Days; 
                    return (dataReferencia - dataLimite.Value.Date).Days; 
            }
            else
            {
                var dataLimite = DataFimPlanejada ?? DataFimReal;
                if (DataFimReal.Value.Date > dataLimite?.Date)
                    return (DataFimReal.Value.Date - dataLimite.Value.Date).Days; 
            }

            return 0;
        }
        private bool EstaAtrasadaMethod()
        {
            if (StatusProjetoId == (int) Enums.StatusProjeto.Concluido || StatusProjetoId == (int) Enums.StatusProjeto.Cancelado) 
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

        // Método para carregar dados do repositório (usado pela infraestrutura)
        public void CarregarDados(int id, string codigo, string nome, int projetoId, int responsavelId,
                                 int tipoAtividadeId, DateTime? dataInicioPlanejada, DateTime? dataFimPlanejada,
                                 DateTime? dataInicioReal, DateTime? dataFimReal, int statusProjetoId,
                                 string? observacoes, string? impedimentos, DateTime dataCadastro)
        {
            Id = id;
            Codigo = codigo;
            Nome = nome;
            ProjetoId = projetoId;
            ResponsavelId = responsavelId;
            TipoAtividadeId = tipoAtividadeId;
            DataInicioPlanejada = dataInicioPlanejada;
            DataFimPlanejada = dataFimPlanejada;
            DataInicioReal = dataInicioReal;
            DataFimReal = dataFimReal;
            StatusProjetoId = statusProjetoId;
            Observacoes = observacoes;
            Impedimentos = impedimentos;
            DataCadastro = dataCadastro;
        }

        public void CarregarComentarios(IEnumerable<AtividadeComentario> comentarios)
        {
            _comentarios.Clear();
            _comentarios.AddRange(comentarios);
        }
    }
}
