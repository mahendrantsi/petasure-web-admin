namespace Project.Models.CommonModel
{
    public class SmtpModel
    {
        public string SmtpUser { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpPassword { get; set; }

        public bool EnableSsl { get; set; }
    }
}
