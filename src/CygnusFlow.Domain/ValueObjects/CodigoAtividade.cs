using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Shared;
using System.Text.RegularExpressions;

namespace CygnusFlow.Domain.ValueObjects
{
    public class CodigoAtividade
    {
        private static readonly Regex CodigoRegex = new(@"^ATV-\d{5}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        private CodigoAtividade(string value)
        {
            Value = value;
        }

        public static Result<CodigoAtividade> Create(string value)
        {
            var validation = Validate(value);
            if (!validation.IsValid)
                return Result<CodigoAtividade>.Failure(validation);

            return Result<CodigoAtividade>.Success(new CodigoAtividade(value));
        }

        public static Result<CodigoAtividade> GerarProximoCodigo(int proximoNumero)
        {
            var codigo = $"ATV-{proximoNumero:D5}";
            return Create(codigo);
        }

        private static NotificationResult Validate(string value)
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(value))
            {
                result.AddError("Codigo", ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Código"), ErrorCodes.REQUIRED);
                return result;
            }

            if (!CodigoRegex.IsMatch(value))
            {
                result.AddError("Codigo", "Código deve estar no formato ATV-00001", ErrorCodes.INVALID_FORMAT);
            }

            return result;
        }

        public override string ToString() => Value;

        public static implicit operator string(CodigoAtividade codigo) => codigo.Value;
    }
}
