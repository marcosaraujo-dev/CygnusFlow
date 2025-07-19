using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Shared;
using System.Text.RegularExpressions;

namespace CygnusFlow.Domain.ValueObjects
{
    public class CodigoProjeto
    {
        private static readonly Regex CodigoRegex = new(@"^PRJ-\d{5}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        private CodigoProjeto(string value)
        {
            Value = value;
        }

        public static Result<CodigoProjeto> Create(string value)
        {
            var validation = Validate(value);
            if (!validation.IsValid)
                return Result<CodigoProjeto>.Failure(validation);

            return Result<CodigoProjeto>.Success(new CodigoProjeto(value));
        }

        public static Result<CodigoProjeto> GerarProximoCodigo(int proximoNumero)
        {
            var codigo = $"PRJ-{proximoNumero:D5}";
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
                result.AddError("Codigo", "Código deve estar no formato PRJ-00001", ErrorCodes.INVALID_FORMAT);
            }

            return result;
        }

        public override string ToString() => Value;

        public static implicit operator string(CodigoProjeto codigo) => codigo.Value;
    }
