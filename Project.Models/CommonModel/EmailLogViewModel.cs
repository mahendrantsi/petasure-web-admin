namespace Project.Models.CommonModel
{
    using System;
    using System.Net.Mail;

    public class EmailLogViewModel
    {
        public long Id { get; set; }

        public string FromMail { get; set; }

        public string ToMail { get; set; }

        public string CcMail { get; set; }

        public string BccMail { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string MailStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SendResult { get; set; }

        public string SendResultId { get; set; }
    }
}
