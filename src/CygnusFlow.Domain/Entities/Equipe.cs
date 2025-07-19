using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Shared;
using System.Collections.Generic;

namespace CygnusFlow.Domain.Entities
{
    public class Equipe
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public NotificationResult Validate()
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(Nome))
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Nome"), ErrorCodes.REQUIRED);
            else if (Nome.Length < 2)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MIN_LENGTH, "Nome", 2), ErrorCodes.MIN_LENGTH);
            else if (Nome.Length > 100)
                result.AddError(nameof(Nome), ErrorMessages.GetMessage(ErrorCodes.MAX_LENGTH, "Nome", 100), ErrorCodes.MAX_LENGTH);

            return result;
        }
    }
}
