namespace Project.Services.ServiceHelper
{
    using System;
    using System.Net.Mail;
    using System.Threading;
    using SendGrid.Helpers.Mail;
    using SendGrid;
    using Project.Models.CommonModel;
    using System.Threading.Tasks;

    public static class EmailHelper
    {
        public static async Task Send(MailViewModel model, SmtpModel smtpModel)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();

                MailAddress fromAddres = new MailAddress(smtpModel.SmtpUser, "Petasure");
                smtpClient.Host = smtpModel.SmtpHost;
                smtpClient.Port = Convert.ToInt32(587);
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpModel.SmtpUser, smtpModel.SmtpPassword);
                smtpClient.EnableSsl = smtpModel.EnableSsl;

                message.From = fromAddres;
                message.To.Add(model.MailTo);
                message.Subject = model.Title;
                message.IsBodyHtml = true;
                message.Body = model.Content;
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //public static async Task<bool> SendEmail(MailViewModel mailViewModel, FromMailViewModel fromModel)
        //{
        //    bool retVal = false;
        //    try
        //    {
        //        var client = new SendGridClient(fromModel.Key);
        //        var from = new EmailAddress(fromModel.FromEmail, fromModel.From);
        //        var to = new EmailAddress(mailViewModel.MailTo);
        //        var msg = MailHelper.CreateSingleEmail(from, to, mailViewModel.Title, string.Empty, mailViewModel.Content);
        //        var response = await client.SendEmailAsync(msg);
        //        retVal = response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        retVal = false;
        //    }

        //    return retVal;
        //}

        //public static bool SendEmail_Old(MailViewModel mailViewModel, SmtpModel smtpModel)
        //{
        //    bool retVal = false;
        //    SmtpClient smtpClient = new SmtpClient() { EnableSsl = true };
        //    MailMessage message = new MailMessage();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(mailViewModel.DisplayName))
        //        {
        //            mailViewModel.DisplayName = "Project";
        //        }

        //        MailAddress fromAddress = new MailAddress(smtpModel.SmtpUser, mailViewModel.DisplayName);
        //        smtpClient.Host = smtpModel.SmtpHost;
        //        smtpClient.Port = smtpModel.SmtpPort;
        //        smtpClient.EnableSsl = smtpModel.EnableSsl;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.Credentials = new System.Net.NetworkCredential(smtpModel.SmtpUser, smtpModel.SmtpPassword);
        //        message.From = fromAddress;
        //        message.To.Add(mailViewModel.MailTo);
        //        if (!string.IsNullOrEmpty(mailViewModel.CC))
        //        {
        //            message.CC.Add(mailViewModel.CC);
        //        }
        //        if (mailViewModel.Attachement != null)
        //        {
        //            foreach (Attachment att in mailViewModel.Attachement)
        //            {
        //                message.Attachments.Add(att);
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(mailViewModel.BCC))
        //        {
        //            message.Bcc.Add(mailViewModel.BCC);
        //        }
        //        message.Subject = mailViewModel.Title;
        //        message.IsBodyHtml = true;
        //        message.Body = mailViewModel.Content;
        //        smtpClient.Send(message);
        //        retVal = true;
        //    }
        //    catch
        //    {
        //        retVal = false;
        //    }
        //    return retVal;
        //}
    }
}
