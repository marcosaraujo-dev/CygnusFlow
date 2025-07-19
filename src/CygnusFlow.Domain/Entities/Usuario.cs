using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Enums;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;

namespace CygnusFlow.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public int? EquipeId { get; set; }
        public TipoUsuario TipoUsuarioId { get; set; }
        public StatusUsuario StatusUsuarioId { get; set; }
        public bool BloqueadoPorRedefinicao { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Equipe? Equipe { get; set; }
        public virtual ICollection<Projeto> Projetos { get; set; } = new List<Projeto>();
        public virtual ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();

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
