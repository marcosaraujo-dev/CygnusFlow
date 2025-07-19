using System.Collections.Generic;
using System.Linq;

namespace CygnusFlow.Domain.Shared
{
    public class NotificationResult
    {
        public List<Error> Errors { get; private set; } = new();
        public List<Warning> Warnings { get; private set; } = new();

        public bool IsValid => !Errors.Any();
        public bool HasWarnings => Warnings.Any();

        public void AddError(string field, string message, string code = "")
        {
            Errors.Add(new Error(field, message, code));
        }

        public void AddWarning(string field, string message, string code = "")
        {
            Warnings.Add(new Warning(field, message, code));
        }

        public void Merge(NotificationResult other)
        {
            Errors.AddRange(other.Errors);
            Warnings.AddRange(other.Warnings);
        }

        public static NotificationResult Success() => new();

        public static NotificationResult Failure(string field, string message, string code = "")
        {
            var result = new NotificationResult();
            result.AddError(field, message, code);
            return result;
        }
    }

    public record Error(string Field, string Message, string Code = "");
    public record Warning(string Field, string Message, string Code = "");
    public record Success   (string Field, string Message, string Code = "");
}
