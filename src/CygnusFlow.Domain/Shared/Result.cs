namespace CygnusFlow.Domain.Shared
{
    public class Result<T>
    {
        public T? Data { get; private set; }
        public NotificationResult Notifications { get; private set; }

        public bool IsSuccess => Notifications.IsValid && Data != null;
        public bool HasWarnings => Notifications.HasWarnings;

        private Result(T? data, NotificationResult notifications)
        {
            Data = data;
            Notifications = notifications;
        }

        public static Result<T> Success(T data, NotificationResult? notifications = null)
        {
            return new Result<T>(data, notifications ?? new NotificationResult());
        }

        public static Result<T> Failure(NotificationResult notifications)
        {
            return new Result<T>(default, notifications);
        }

        public static Result<T> Failure(string field, string message, string code = "")
        {
            var notifications = NotificationResult.Failure(field, message, code);
            return new Result<T>(default, notifications);
        }
    }
}
