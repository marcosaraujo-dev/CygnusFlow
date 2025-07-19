using System.Collections.Generic;

namespace CygnusFlow.Domain.Shared
{
    public class ResultList<T>
    {
        public List<T> Items { get; private set; } = new();
        public int TotalCount { get; private set; }
        public NotificationResult Notifications { get; private set; }

        public bool IsSuccess => Notifications.IsValid;

        private ResultList(List<T> items, int totalCount, NotificationResult notifications)
        {
            Items = items;
            TotalCount = totalCount;
            Notifications = notifications;
        }

        public static ResultList<T> Success(List<T> items, int? totalCount = null)
        {
            return new ResultList<T>(items, totalCount ?? items.Count, new NotificationResult());
        }

        public static ResultList<T> Failure(NotificationResult notifications)
        {
            return new ResultList<T>(new List<T>(), 0, notifications);
        }
    }
}
