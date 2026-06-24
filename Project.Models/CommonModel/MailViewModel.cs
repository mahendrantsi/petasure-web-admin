using System.Net.Mail;

namespace Project.Models.CommonModel
{
    public class MailViewModel
    {
        public string MailTo { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Message { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Attachment[]? Attachement { get; set; }
        public string DisplayName { get; set; } 
    }

    public class FromMailViewModel
    {
        public string FromEmail { get; set; }
        public string From { get; set; }
        public string Key { get; set; }
    }
}
