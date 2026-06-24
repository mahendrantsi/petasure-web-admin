namespace Project.Core.Model
{
    public class NotificationDataModel
    {
        public string to { get; set; }
        public Notifications notification { get; set; }
        public Datas data { get; set; }
    }

    public class Notifications
    {
        public string body { get; set; }
        public string title { get; set; }
        public string badge { get; set; }
        public string sound { get; set; }
        public bool mutable_content { get; set; }
        public string mediaUrl { get; set; }
        public bool content_available { get; set; }
        public string mediaType { get; set; }
        public string image { get; set; }
        public string category { get; set; }
        public string click_action { get; set; }
    }

    public class Datas
    {
        public string category { get; set; }
        public long notificationId { get; set; }
        public int eventId { get; set; }
    }

    public class NotificationResponse
    {
        public string multicast_id { get; set; }

        public int success { get; set; }

        public int failure { get; set; }
    }
}