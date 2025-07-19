using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Entities
{
    public class ModuloSistema
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public virtual ICollection<Projeto> Projetos { get; set; } = new List<Projeto>();

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
