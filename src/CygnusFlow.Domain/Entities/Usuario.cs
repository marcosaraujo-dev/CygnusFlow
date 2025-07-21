using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CygnusFlow.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 255 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public int? EquipeId { get; set; }
        public virtual Equipe? Equipe { get; set; }

        public int TipoUsuarioId { get; set; }
        public virtual TipoUsuario TipoUsuario { get; set; } = null!;

        public int StatusUsuarioId { get; set; }
        public virtual StatusUsuario StatusUsuario { get; set; } = null!;
        public bool BloqueadoPorRedefinicao { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public virtual ICollection<Projeto> ProjetosResponsavel { get; set; } = new List<Projeto>();
        public virtual ICollection<Atividade> AtividadesResponsavel { get; set; } = new List<Atividade>();
        public virtual ICollection<ProjetoComentario> ProjetoComentarios { get; set; } = new List<ProjetoComentario>();
        public virtual ICollection<AtividadeComentario> AtividadeComentarios { get; set; } = new List<AtividadeComentario>();


        public NotificationResult Validate()
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(Nome))
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Nome"), ErrorCodes.REQUIRED);
            else if (Nome.Length < 2)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MIN_LENGTH, "Nome", 2), ErrorCodes.MIN_LENGTH);
            else if (Nome.Length > 150)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Nome", 150), ErrorCodes.MAX_LENGTH);

            if (string.IsNullOrWhiteSpace(Email))
                result.AddError(nameof(Email), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "E-mail"), ErrorCodes.REQUIRED);
            else if (!IsValidEmail(Email))
                result.AddError(nameof(Email), ErrorMessages.GetMessage(ErrorCodes.INVALID_FORMAT, "E-mail"), ErrorCodes.INVALID_FORMAT);

            if (string.IsNullOrWhiteSpace(SenhaHash))
                result.AddError(nameof(SenhaHash), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Senha"), ErrorCodes.REQUIRED);

            return result;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
